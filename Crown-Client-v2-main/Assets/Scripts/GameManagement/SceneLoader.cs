using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//based on Code Monkey's scene manager tutorial: https://www.youtube.com/watch?v=3I5d2rUJ0pE
public enum SceneNames
{
    MainMenu,
    RegistrationScene,
    GameScene,
    LoadingScence,
    CritterEncounter,
    DefaultCapture,
    LoadingScene,
    TeamScene,
    StackingGame,
    StudySession
}
public static class SceneLoader 
{
    
    public static void Load(SceneNames _scene)
    {
        SceneManager.LoadScene(_scene.ToString());
    }

    public static int returnCurrentScene()
    {
          return SceneManager.GetActiveScene().buildIndex;
    }
}
