using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//from Tom Weiland's networking tutorial: https://www.youtube.com/playlist?list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //dictionary to store players
    //might be needed later if we decide to display other players on map
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public MapManager mapManager;
    public MapCamera mapCamera;
    public QuestManager questManager;
    public PlayerController playerController;
    public PlayerManager player;
    public GameSceneUIManager uiManager;
    public Camera mainCamera;
    public GameObject critterContainer;
    private TeamsMenuUIManager teamsUIManager;

    //records the state of the game
    //0 = base state
    //1 = overhead map open
    private int gameState = 0;
    
    
    private void Awake()
    //initialize the game manager singleton upon waking up
    {      

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance alreasy exists; destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {        
        //initially set this command to block 
        ClientSend.AddQueueCommand(new SendStuffCommand());
        //ClientSend.AddQueueCommand(new BlockingCommand());
        ClientSend.AddQueueCommand(new RequestActiveQuestsCommand());
        //ClientSend.AddQueueCommand(new BlockingCommand());
        ClientSend.AddQueueCommand(new RequestAvaliableQuestsCommand());
        //ClientSend.AddQueueCommand(new SpawnCrittersCommand(false));
    }

    public void SpawnPlayer(int _xp, int _energyCarried, string _team)
    {
        Debug.Log($"Spawn player called");
        player.setXP(_xp);
        player.setEnergyCarried(_energyCarried);
        player.setTeam(_team);
        
    }

    public void PopulateGameScene()
    {
        //initially set this command to block 
        ClientSend.AddQueueCommand(new SendStuffCommand());
        //ClientSend.AddQueueCommand(new BlockingCommand());
        ClientSend.AddQueueCommand(new RequestActiveQuestsCommand());
        //ClientSend.AddQueueCommand(new BlockingCommand());
        ClientSend.AddQueueCommand(new RequestAvaliableQuestsCommand());
        //ClientSend.AddQueueCommand(new SpawnCrittersCommand(false));
    }

    public void DisplayOverHeadMap()
    {

        //set gameState to 1
        gameState = 1;

        //activate the mapCamera's game object and call its activate method.
        mapCamera.gameObject.SetActive(true);
        mapCamera.Activate();
        //scale the map markers up
        //TODO figure out what markers to scale up and how
        mapManager.ScaleMarkers(10);

        //Client.instance.Disconnect();

        
    }

    public void CloseOverHeadMap()
    {
        //set gameState to 0
        gameState = 0;

        //call the mapCamera's deactivate method and deactivate its gameobject;
        mapCamera.DeActivate();
        mapCamera.gameObject.SetActive(false);

        //scale the map markers down
        mapManager.ScaleMarkers(1);

    }
    public void ChangeScene(string _sceneName)
    {
        //SceneLoader.Load((int)SceneNames._scene);
        DisableGameSceneElements();
        SceneManager.LoadScene(_sceneName,LoadSceneMode.Additive);
        //SceneManager.LoadScene(_sceneName);
    }

    public void LoadNewScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    //called to send request to server to launch a minigame
    public void LoadMiniGame(string _gametype)
    {
        ClientSend.AddQueueCommand(new LoadMiniGameCommand(_gametype));  
    }

    //called when minigames and other scenes loaded additively
    private void DisableGameSceneElements()
    {
        //mainCamera.gameObject.SetActive(false);
        mapManager.gameObject.SetActive(false);
        uiManager.gameObject.SetActive(false);
        critterContainer.SetActive(false);
        
    }

    private void EnableGameSceneElements()
    {
        mainCamera.gameObject.SetActive(true);
        mapManager.gameObject.SetActive(true);
        uiManager.gameObject.SetActive(true);
        critterContainer.SetActive(true);
    }

    public void UnloadScene(string _sceneName)
    {
        //SceneManager.LoadScene("GameScene");
        
        SceneManager.UnloadSceneAsync(_sceneName);
        EnableGameSceneElements();
    }

    public int returnGameState()
    {
        return gameState;
    }

    public void SetTeamMenuUIManager(TeamsMenuUIManager _manager)
    {
        teamsUIManager = _manager;
    }

    public TeamsMenuUIManager ReturnTeamsMenuUIManager()
    {
        return teamsUIManager;
    }


}
