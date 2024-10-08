using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMiniGameCommand : NetworkCommand
{
    //sends a request to the server to launch a minigame. The server will then return configuration instructions for the type of game requested
    public LoadMiniGameCommand(string _gameType)
    {
        packet = new Packet((int)ClientPackets.loadMiniGame);
        packet.Write(_gameType);
        packetId = (int)ClientPackets.loadMiniGame;
    }
}
