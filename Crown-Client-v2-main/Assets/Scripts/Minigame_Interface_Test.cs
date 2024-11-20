using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public interface IMinigame
{
    public void IGameSetup();
    public void IFinishGame();

    public void IUpdatePlayerData(PlayerData playerData);

    public void IPlayGame();
    public void IStopGame();
    

}  
