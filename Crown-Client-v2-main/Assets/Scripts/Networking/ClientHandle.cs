using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

//from Tom Weiland's networking tutorial: https://www.youtube.com/playlist?list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5

public class ClientHandle : MonoBehaviour
{

    //called after client initally connects and the server responds

    //TODO MAKE THE WELCOME COMMAND ONLY VERIFY INITIAL CONNECTION.
    //IF PLAYER IS RECONNECTING TO SEND DATA, THEN IT SENDS NEXT IN QUEUE
    //IF NOT IT JUST WAITS UNTIL CLIENT SENDS SPECIFIC REQUESTS WITH SPECIFIC PACKETS SUCH AS GOOGLELOGIN, REGISTERETC...
    //WELCOME COMMAND UNLOCKS QUEUE SO NEXT CAN BE SENT
    /*
    public static void Welcome (Packet _packet)
    {
        //must read the components in the packet in the same order they were added
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        int _connectionContext = Client.instance.connectionContext;

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;

        //if the client is in the game scene, then insert the welcome recieved command before any others currently in the queue.
        //this is because the client has reconnected in preparation for sending another command
        if (SceneLoader.returnCurrentScene() == (int)SceneNames.GameScene)
        {
            ClientSend.AddQueueCommand(0, new ReconnectCommand());
        }
        else
        {   
            ClientSend.AddQueueCommand(new WelcomeCommand());
        }

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);

        //todo Send welcome recieved response
    }
    */
    public static void Welcome(Packet _packet)
    {
        //must read the components in the packet in the same order they were added
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;

        //connect the udp listener / socket (might not need)
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);

        //everytime the client connects of reconnects, the send queue is locked until this welcome message is recieved and the client's server ID is set
        //so send unblock queue once that is done.
        NetworkCommand _loginCommand = ManageLogin.instance.ReturnLoginCommand();
        ClientSend.ResetRequestTimer(1);
        ClientSend.AddQueueCommand(_loginCommand);
        //ClientSend.UnlockQueueCommand();

    }

    public static void GoogleLoginData(Packet _packet)
    {
        string _monarchProjectClientID = _packet.ReadString();
        string __clientStateToken = _packet.ReadString();

        Debug.Log("google login data received " + Client.instance.myId);
        Oauth2Manager.GetAuthCode(_monarchProjectClientID, __clientStateToken);
    }

    public static void LaunchGameScene(Packet _packet)
    {
        Client.instance.username = _packet.ReadString();
        Client.instance.password = _packet.ReadString();
        SceneLoader.Load(SceneNames.GameScene);
        Client.instance.connectionContext = 2;
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _xp = _packet.ReadInt();
        int _energyCarried = _packet.ReadInt();
        string _team = _packet.ReadString();
        Debug.Log("TEAM = " + _team);
        GameManager.instance.SpawnPlayer(_xp, _energyCarried, _team);


    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        GameManager.players[_id].transform.position = _position;
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }


    public static void SafeToDisconnect(Packet _packet)
    {
        //Debug.Log("Safe to Disconnect Received!!!");
        //ClientSend.AddQueueCommand(new DisconnectCommand());
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void LoginFailed(Packet _packet)
    {
        Debug.Log("Login failed!");
        //clear the textboxes
        UIManager.instance.loginPanel.ResetInputFields();
        //display error message on login panel
        ErrorUIManager.instance.initialize(1);
        //disconnect from server
        Client.instance.Disconnect();

    }

    public static void RegistrationFailed(Packet _packet)
    {
        Debug.Log("Registration failed!");
        //clear the textboxes
        RegistrationManager.instance.createAccountPanel.ResetInputFields();
        //display error message on login panel
        RegistrationManager.instance.createAccountPanel.ToggleErrorText(true);
        //disconnect from server
        Client.instance.Disconnect();

    }

    public static void KeepAliveReceived(Packet _packet)
    {
        if (SceneLoader.returnCurrentScene() == 2)
        {
            GameManager.instance.uiManager.PingReceived();
        }
    }

    public static void UnlockQueueCommand(Packet _packet)
    {
        ClientSend.UnlockQueueCommand();
    }
    public static void AccountCreated(Packet _packet)
    {
        int _step = _packet.ReadInt();
        Debug.Log($"Account creation is on {_step}!");
        //clear the textboxes
        RegistrationManager.instance.AdvanceAccountCreation(_step);

    }

    //handle the packet from the server with base details for an individual quest
    public static void AvaliableQuestDetails(Packet _packet)
    {
        QuestClass _quest = new QuestClass();
        _quest.questId = _packet.ReadInt();
        _quest.questName = _packet.ReadString();
        _quest.questDescription = _packet.ReadString();
        _quest.questType = _packet.ReadInt();
        _quest.questDuration = _packet.ReadInt();
        _quest.questSpecialization = _packet.ReadInt();
        _quest.isQuestRepeatiable = _packet.ReadBool();
        _quest.xpAwarded = _packet.ReadInt();
        _quest.energyAwarded = _packet.ReadInt();
        _quest.displayLat = _packet.ReadString();
        _quest.displayLong = _packet.ReadString();
        _quest.totalQuestSteps = _packet.ReadInt();

        //Debug.Log($"Quest Received : ID = {_quest.questId} Name = {_quest.questName} Duration = {_quest.questDuration} Repeatiable? {_quest.isQuestRepeatiable} XP {_quest.xpAwarded}  ENERGY {_quest.energyAwarded} LAT {_quest.displayLat} Long {_quest.displayLong} ");

        //have the quest manager add quest to the list of avaliable quests
        AddAvaliableQuestCommand _command = new AddAvaliableQuestCommand(_quest);
        GameManager.instance.questManager.AddQuestQueueCommand(_command);
        //GameManager.instance.questManager.AddAvaliableQuest(_quest);

    }

    public static void ActiveQuestDetails(Packet _packet)
    {
        QuestClass _quest = new QuestClass();
        _quest.questId = _packet.ReadInt();
        _quest.questName = _packet.ReadString();
        _quest.questDescription = _packet.ReadString();
        _quest.questType = _packet.ReadInt();
        _quest.questDuration = _packet.ReadInt();
        _quest.questSpecialization = _packet.ReadInt();
        _quest.isQuestRepeatiable = _packet.ReadBool();
        _quest.xpAwarded = _packet.ReadInt();
        _quest.energyAwarded = _packet.ReadInt();
        _quest.displayLat = _packet.ReadString();
        _quest.displayLong = _packet.ReadString();
        _quest.totalQuestSteps = _packet.ReadInt();
        _quest.currentStep = _packet.ReadInt();

        //Debug.Log($"ACTIVE Quest Received : ID = {_quest.questId} Name = {_quest.questName} Total Steps = {_quest.totalQuestSteps} Current Step = {_quest.currentStep} ");

        for (int i = 1; i <= _quest.totalQuestSteps; i++)
        {
            QuestStep _questStep = new QuestStep();
            _questStep.lattitude = _packet.ReadString();
            _questStep.longitude = _packet.ReadString();
            _questStep.action = _packet.ReadString();
            _questStep.npc = _packet.ReadInt();
            _questStep.text = _packet.ReadString();

            //Debug.Log($"Writing quest step {i} Lat = {_questStep.lattitude} long = {_questStep.longitude} Text = {_questStep.text}");

            _quest.questSteps.Add(_questStep);
        }

        //Debug.Log($"ACTIVE Quest Received : ID = {_quest.questId} Name = {_quest.questName} ");

        AddActiveQuestCommand _command = new AddActiveQuestCommand(_quest);
        GameManager.instance.questManager.AddQuestQueueCommand(_command);

        //GameManager.instance.questManager.QuestAdded(_quest);
    }

    public static void QuestAdvanced(Packet _packet)
    {
        int _questId = _packet.ReadInt();
        int _nextStep = _packet.ReadInt();

        QuestAdvancedCommand _command = new QuestAdvancedCommand(_questId, _nextStep);
        GameManager.instance.questManager.AddQuestQueueCommand(_command);

        //GameManager.instance.questManager.QuestAdvanced(_questId, _nextStep);
    }

    public static void QuestFinished(Packet _packet)
    {
        int _questId = _packet.ReadInt();
        GameManager.instance.player.setXP(_packet.ReadInt());
        GameManager.instance.player.setEnergyCarried(_packet.ReadInt());

        QuestRemovedCommand _command = new QuestRemovedCommand(_questId);
        GameManager.instance.questManager.AddQuestQueueCommand(_command);

        //GameManager.instance.questManager.QuestFinished(_questId);
    }


    public static void QuestRemoved(Packet _packet)
    {

        int _questId = _packet.ReadInt();
        Debug.Log("Removing Quest " + _questId);

        QuestRemovedCommand _command = new QuestRemovedCommand(_questId);
        GameManager.instance.questManager.AddQuestQueueCommand(_command);

        //GameManager.instance.questManager.QuestRemoved(_questId);
    }

    //sent from server to acknowledge receipt of a packet by the client. Will help client
    //determine if network connection has been interrupted.
    public static void PacketReceived(Packet _packet)
    {
        int _packetId = _packet.ReadInt();
        ClientSend.ResetRequestTimer(_packetId);
    }

    //confirmation that team has been created
    public static void TeamCreated(Packet _packet)
    {
        string _teamName = _packet.ReadString();
        //request details about new team, as this will cause the team scene to display the team details menu
        ClientSend.AddQueueCommand(new RequestTeamDetailsCommand(_teamName));
        Debug.Log("Team " + _teamName + "created!!!");
    }

    public static void TeamDetailsReceived(Packet _packet)
    {
        TeamDetailsClass _teamDetails = new TeamDetailsClass();
        _teamDetails.teamName = _packet.ReadString();
        _teamDetails.teamOwner = _packet.ReadString();
        _teamDetails.teamCreatedOn = _packet.ReadInt();
        _teamDetails.teamEnergy = _packet.ReadInt();
        _teamDetails.teamDescription = _packet.ReadString();
        _teamDetails.teamFocus = _packet.ReadString();
        _teamDetails.teamLevel = _packet.ReadInt();
        _teamDetails.totalTeamMembers = _packet.ReadInt();

        //Debug.Log(_teamDetails.teamName + " " + _teamDetails.teamOwner + " " +
        //_teamDetails.teamCreatedOn + " " + _teamDetails.teamEnergy + " " + _teamDetails.teamDescription +
        //" " + _teamDetails.teamFocus + " " + _teamDetails.teamLevel);

        //if the find teams panel is open, then use the team details class to create buttons for the join teams listing
        if (GameManager.instance.ReturnTeamsMenuUIManager().findTeamPanel.isActiveAndEnabled)
        {
            CreateTeamButtonCommand _command = new CreateTeamButtonCommand(_teamDetails);
            GameManager.instance.ReturnTeamsMenuUIManager().AddTeamQueueCommand(_command);
        }
        //otherwise display the details of the player's team; set context to 0 to indicate that it was called on startup
        else
        {
            GameManager.instance.ReturnTeamsMenuUIManager().PopulateTeamDetailsPanel(_teamDetails, 0);
        }

    }

    public static void TeamMemberDetailsReceived(Packet _packet)
    {
        TeamMemberDetailClass _member = new TeamMemberDetailClass();

        _member.userName = _packet.ReadString();
        _member.firstName = _packet.ReadString();
        _member.lastName = _packet.ReadString();
        _member.tagLine = _packet.ReadString();
        _member.joinDate = _packet.ReadInt();
        _member.level = _packet.ReadInt();
        _member.xp = _packet.ReadInt();
        _member.rank = _packet.ReadInt();
        _member.luck = _packet.ReadInt();
        _member.energy = _packet.ReadInt();
        _member.grit = _packet.ReadInt();
        _member.empathy = _packet.ReadInt();
        _member.nerve = _packet.ReadInt();
        _member.dexterity = _packet.ReadInt();

        CreateMemberButtonCommand _command = new CreateMemberButtonCommand(_member);
        GameManager.instance.ReturnTeamsMenuUIManager().AddTeamQueueCommand(_command);

        //Debug.Log("TEAM meber info received for " + _member.userName + " " + _member.firstName + " " + _member.lastName);
    }

    public static void TeamMemberAdded(Packet _packet)
    {
        string _teamMember = _packet.ReadString();
        string _teamName = _packet.ReadString();

        Debug.Log("TEAM MEMBER ADDED CALLED " + _teamMember + " " + _teamName);
    }

    public static void LoadMiniGame(Packet _packet)
    {
        string _gameType = _packet.ReadString();
        string _miniGameHash = _packet.ReadString();

        Client.instance.miniGameHash = _miniGameHash;
        SceneLoader.Load((SceneNames)Enum.Parse(typeof(SceneNames),_gameType));
    }

    public static void ExitMiniGame(Packet _packet)
    {
        SceneLoader.Load(SceneNames.GameScene);
    }
}
