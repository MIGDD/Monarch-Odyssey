using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Test_Minigame : IMinigame
{
    public void IGameSetup()
    {
        //code to prepare the game like moving text boxes and unlocking the play button once everything is ready
    }
    public void IPlayGame()
    {
        //code to get the minigame to play, will be called once play button is pressed
    }
    public void IStopGame()
    {
        //Used to disable user input and display results; called once the minigame is over
    }
    public void IUpdatePlayerData(PlayerData playerData)
    {
        //code to update the playerData file once the minigame is complete
    }
    public void IFinishGame()
    {
        //will likely be the same for all minigames and will bring the player back to the main game once the exit/end button has been pressed
        SceneManager.LoadScene("GameScene");
    }

    
    
    
    
}
