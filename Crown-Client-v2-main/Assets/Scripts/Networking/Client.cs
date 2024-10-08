using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System;

//from Tom Weiland's networking tutorial: https://www.youtube.com/playlist?list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;

    public bool useLocalIP = false;

    public string localIP = "127.0.0.1";
    public string serverIP = "127.0.0.1";
    public int port = 26950;
    public int myId = 0;
    public TCP tcp;
    public UDP udp;

    public string username;
    public string password;

    public string miniGameHash;

    public bool isConnected = false;
    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    public int connectionContext;

    public enum ContextForConnection
    {
        Login = 0,
        Register,
        Reconnect,
        GoogleLogin,
        GoogleRegistration
    }

    private void Awake()
    //initialize the client singleton upon waking up

    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Debug.Log("Instance alreasy exists; destroying object!");
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeClientData();
        connectionContext = (int)ContextForConnection.Login;
        //InvokeRepeating("SendKeepAlive", 60f, 60f);
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void ConnectToServer()
    {
        tcp = new TCP();
        udp = new UDP();
        isConnected = true;
        tcp.Connect();
    }

    //handles TCP connections
    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void Connect()
        {

            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];

            IAsyncResult _result;
            if (instance.useLocalIP)
            {
                _result = socket.BeginConnect(instance.localIP, instance.port, ConnectCallback, socket);
            } else
            {
                _result = socket.BeginConnect(instance.serverIP, instance.port, ConnectCallback, socket);
            }

            var success = _result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

            if (!success)
            {
                //ClientSend.ClearCommandQueue();
                ErrorUIManager.instance.initialize(0);
                instance.isConnected = false;
                instance.Disconnect();
            }

        }
        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
            {

                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }     



        public void SendData(Packet _packet)
        {

            try
            {
                if (socket != null)
                {
                    //before sending write the length of the packet to the packet
                    //this was originally done in the client send method, but moved it here
                    //so that executing a network command did not rewrite the packet length
                    //everytime execute was called on the command, which could happen multiple time
                    //if the client disconnected and retried sending the command
                    _packet.WriteLength();
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);                    
                }

            }
            catch (Exception ex) {
                Debug.Log($"Error sending data to Server via TCP: {ex}");
                //TODO tell client send to stop trying!!


            }
        }
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    instance.Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            }
            catch 
            {
                //Debug.Log("ERRORR Disconnecting!!");
                Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;
            receivedData.SetBytes(_data);

            //if packet received data is greater than the length of an int, the first byte we send, then it contains something
            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        
                        //Debug.Log($"Packet ID = {_packetId} " + Enum.GetName(typeof(ServerPackets), _packetId));
                        packetHandlers[_packetId](_packet);

                        

                    }

                });

                
                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }

                }

            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false; 
        }

        private void Disconnect()
        {
            instance.Disconnect();
            
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }

    }

    public class UDP
    {
        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.serverIP), instance.port);

        }

        public void Connect(int _localPort)
        {
            socket = new UdpClient(_localPort);

            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallback, null);

            using (Packet _packet = new Packet())
            {
                SendData(_packet);
            }

        }

        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(instance.myId);
                if (socket != null)
                {
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                }
            } catch (Exception ex)
            {
                Debug.Log($"Error sending data via UDP: {ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                if (_data.Length < 4)
                {
                    instance.Disconnect();
                    return;
                }

                HandleData(_data);
            } catch 
            {
                //Debug.Log($"Error sending data via UDP: {_ex}");
                Disconnect();
            }
        }

        private void HandleData(byte[] _data)
        {
            using (Packet _packet = new Packet(_data))
            {
                int _packetLength = _packet.ReadInt();
                _data = _packet.ReadBytes(_packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_data))
                {
                    int _packetId = _packet.ReadInt();
                    packetHandlers[_packetId](_packet);

                }

            });
        }

        private void Disconnect()
        {
            instance.Disconnect();
                            
            endPoint = null;
            socket = null;
        }
    }
    private void InitializeClientData()
    {
        //dictionary that stores packet types from the server
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int)ServerPackets.welcome, ClientHandle.Welcome },
            {(int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer },
            {(int)ServerPackets.playerPosition, ClientHandle.PlayerPosition },
            {(int)ServerPackets.playerRotation, ClientHandle.PlayerRotation },
            {(int)ServerPackets.safeToDisconnect, ClientHandle.SafeToDisconnect},
            {(int)ServerPackets.playerDisconnected, ClientHandle.PlayerDisconnected },
            {(int)ServerPackets.loginFailed, ClientHandle.LoginFailed },
            {(int)ServerPackets.registrationFailed, ClientHandle.RegistrationFailed },
            {(int)ServerPackets.accountCreated, ClientHandle.AccountCreated },
            {(int)ServerPackets.launchGameScene, ClientHandle.LaunchGameScene },
            {(int)ServerPackets.avaliableQuestDetails, ClientHandle.AvaliableQuestDetails },
            {(int)ServerPackets.activeQuestDetails, ClientHandle.ActiveQuestDetails },
            {(int)ServerPackets.questAdvanced, ClientHandle.QuestAdvanced },
            {(int)ServerPackets.questFinished, ClientHandle.QuestFinished },
            {(int)ServerPackets.questRemoved, ClientHandle.QuestRemoved },
            {(int)ServerPackets.keepAliveReceived, ClientHandle.KeepAliveReceived },
            {(int)ServerPackets.unlockQueueCommand, ClientHandle.UnlockQueueCommand },
            {(int)ServerPackets.googleLoginData, ClientHandle.GoogleLoginData },
            {(int)ServerPackets.packetReceived, ClientHandle.PacketReceived },
            {(int)ServerPackets.teamCreated, ClientHandle.TeamCreated },
            {(int)ServerPackets.teamDetails, ClientHandle.TeamDetailsReceived },
            {(int)ServerPackets.teamMember, ClientHandle.TeamMemberDetailsReceived },
            {(int)ServerPackets.teamMemberAdded, ClientHandle.TeamMemberAdded },
            {(int)ServerPackets.loadMiniGame, ClientHandle.LoadMiniGame },
            {(int)ServerPackets.exitMiniGame, ClientHandle.ExitMiniGame }

        };
        Debug.Log("Initialized Packets");
    }

    

    public void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            //tcp.socket.GetStream().Close();
            tcp.socket.Close();
            udp.socket.Close();
            
            Debug.Log("Disconnected from server.");
        }
        
    }

    private void SendKeepAlive()
    {
        if (isConnected)
        {
            //ClientSend.KeepAlive();
        }
    }

}
