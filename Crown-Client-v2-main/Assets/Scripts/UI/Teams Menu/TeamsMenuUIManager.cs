using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamsMenuUIManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject noTeamPanel;
    public FindTeamPanel findTeamPanel;
    public GameObject createTeamPanel;
    //public TeamMembersPanel teamMembersPanel;
    public TeamDetailsPanelManager teamDetailsPanel;
    //public MemberDetailsPanel memberDetailsPanel;
    
    //prefabs and scroll rects. TODO: MOVE THESE TO SEPERATE PANEL MANAGERS
    

    public GameObject teamListingButtonPrefab;
    public GameObject teamListingScrollRectContainer;

    //queue to hold and process team commands from the server
    public List<TeamCommand> teamCommandQueue = new List<TeamCommand>();

    private string playerTeam;
    // Start is called before the first frame update
    void Start()
    {
        //set the team manager UI menu value on the gamemanager
        GameManager.instance.SetTeamMenuUIManager(this);
        //configure initial state of the UI based on whether or not the player is on a team
        noTeamPanel.SetActive(false);
        findTeamPanel.gameObject.SetActive(false);
        createTeamPanel.SetActive(false);
        teamDetailsPanel.gameObject.SetActive(false);
        //memberDetailsPanel.gameObject.SetActive(false);
       

        playerTeam = GameManager.instance.player.returnTeamName();
        Debug.Log("TEAM SCENE TEAM NAME = " + playerTeam);
        //if player does not belong to a team, show the no team options menu
        if (playerTeam == "")
        {
            noTeamPanel.SetActive(true);
        } else
        //if player belongs to a team, show the team details menu
        {
            //request details about the team from server
            ClientSend.AddQueueCommand(new RequestTeamDetailsCommand(playerTeam));            
        }
    }

    //update processes queue commands from server in order they are received to avoid race conditions.
    private void Update()
    {
        //if there are commands in the queue execute them
        if (teamCommandQueue.Count > 0)
        {
            TeamCommand _currentCommand = teamCommandQueue[0];
            //process team commands in order then remove them
            if (!_currentCommand.IsProcessing())
            {
                Debug.Log("PROCESSING Team COMMAND " + _currentCommand.GetType() + " items in queue: " + teamCommandQueue.Count);
                _currentCommand.Process();
                teamCommandQueue.Remove(_currentCommand);
            }
        }
    }

    //add command to queue;
    public void AddTeamQueueCommand(TeamCommand _teamCommand)
    {
        Debug.Log("ADDING TEAM COMMAND " + _teamCommand.GetType());
        teamCommandQueue.Add(_teamCommand);
    }

    //remove command to queue;
    public void RemoveTeamQueueCommand(TeamCommand _teamCommand)
    {
        Debug.Log("Removing TEAM COMMAND " + _teamCommand.GetType());
        teamCommandQueue.Remove(_teamCommand);

    }

    //called when client needs to display info about a team. NEED TO ADD CONTEXT VARIABLE!!
    public void PopulateTeamDetailsPanel(TeamDetailsClass _teamDetails, int _context)
    {        
        teamDetailsPanel.Initialize(_teamDetails, _context);
        teamDetailsPanel.gameObject.SetActive(true);
        noTeamPanel.SetActive(false);
        findTeamPanel.gameObject.SetActive(false);
        //teamMembersPanel.gameObject.SetActive(false);
        createTeamPanel.SetActive(false);
        //memberDetailsPanel.gameObject.SetActive(false);
        
    }

    public void CreateTeamClicked()
    {
        noTeamPanel.SetActive(false);
        findTeamPanel.gameObject.SetActive(false);
        createTeamPanel.SetActive(true);
        //teamMembersPanel.gameObject.SetActive(false);
        teamDetailsPanel.gameObject.SetActive(false);
        //memberDetailsPanel.gameObject.SetActive(false);
        
    }

    //caled when player clicks "Find Team" from main menu; 
    public void FindTeamClicked()
    {
        noTeamPanel.SetActive(false);
        findTeamPanel.gameObject.SetActive(true);
        findTeamPanel.Initialize();
        createTeamPanel.SetActive(false);
        //teamMembersPanel.gameObject.SetActive(false);
        teamDetailsPanel.gameObject.SetActive(false);
        //memberDetailsPanel.gameObject.SetActive(false);
        
    }

    
    //called when a team button in the listing of avaliable teams is clicked. Set context to 1 to tell client that the request originated from find teams menu
    public void TeamButtonClicked(TeamDetailsClass _team)
    {
        noTeamPanel.SetActive(false);
        findTeamPanel.gameObject.SetActive(false);
        createTeamPanel.SetActive(false);
        //teamMembersPanel.gameObject.SetActive(false);
        teamDetailsPanel.gameObject.SetActive(false);
        //memberDetailsPanel.gameObject.SetActive(true);
        PopulateTeamDetailsPanel(_team, 1);
    }

    public void CreateTeamButton(TeamDetailsClass _team)
    {


        Debug.Log("CREATE TEAM BUTTON CALLED FOR " + _team.teamName);
        findTeamPanel.ToggleTeamListingPanel();
        GameObject _teamButton = Instantiate(teamListingButtonPrefab, teamListingScrollRectContainer.transform);
        TeamListingButton _buttonScript = _teamButton.GetComponent<TeamListingButton>();
        _buttonScript.Initialize(_team);
    }

    public void DeleteOldButtons(Transform _containerObject)
    {
        foreach (Transform _child in _containerObject)
        {
            _child.gameObject.Destroy();
        }
    }

    public void ExitClicked()
    {
        //mainCamera.gameObject.SetActive(false);
        GameManager.instance.UnloadScene("TeamsMenu"); 
    }

    //method manages what happens when players press the back button from various sub menus when the client needs to show one main menu and hide another 
    //i.e. when players click back from the team overview UI to return to the search teams listing UI
    public void BackClicked(int _context)
    {
        if (_context == 0) // players back out of team overview UI panel they clicked on from the search teams scroll view UI
        {
            findTeamPanel.gameObject.SetActive(true);
            teamDetailsPanel.gameObject.SetActive(false);

        }
    } 
}
