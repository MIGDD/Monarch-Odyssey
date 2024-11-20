using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData: MonoBehaviour
{
    //Player Information
    private string playerDisplayName;
    private int playerID;

    //Points/Score
    private double totalScore;
    
    //Creature Stats
    private int creaturesCaptured;
    private int creaturesAttemptCapture;

    //Quest Stats
    private int questsCompleted;
    private int questsAttempted;

    //Building Stats
    private int buildingsCaptured;
    private int buildingsAttemptCapture;

    //Team Information
    private bool isOnTeam;
    private string teamName;
    private string teamRole;

    //Online Status
    private bool isOnline;

    //Time Played
    private double timePlayed;

    //minigame top scores (need one for each minigame)
    private double minigameScore;


}
