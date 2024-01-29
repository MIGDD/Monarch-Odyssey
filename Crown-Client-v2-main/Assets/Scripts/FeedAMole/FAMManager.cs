using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAMManager : MonoBehaviour
{
    //List of moles
    [SerializeField] private List<MoleBehavior> moles;

    [Header("UI Objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject outOfTime;
    [SerializeField] private GameObject finalScore;
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI finalScoreText;

    //Hardcoded variables
    private float startingTime = 60f;

    //Global Variables
    private float remainingTime; //for the game
    //To keep track of which moles are active so other ones can be activated
    private HashSet<MoleBehavior> currentMoles = new HashSet<MoleBehavior>();
    private int score;
    private bool playing = false;

    public void startGame()
    {
        //Hide/Show the UI elements we don't/do want to see
        playButton.SetActive(false);
        outOfTime.SetActive(false);
        finalScore.SetActive(false);
        gameUI.SetActive(true);
        //Hide all the visible moles
        for(int i=0; i< moles.Count; i++)
        {
            moles[i].Hide();
            moles[i].setIndex(i);
        }
        //Remove any old game state
        currentMoles.Clear();
        //Start with 60 seconds
        remainingTime = startingTime;
        score = 0;
        scoreText.text = "0";
        playing = true;
        timerText.color = Color.white;

    }

    //Function to handle game over logic
    public void GameOver()
    {
        //Make game over scene active
        outOfTime.SetActive(true);
        finalScore.SetActive(true);
        gameUI.SetActive(false);
        finalScoreText.text = $"{score}";
        //Hide all moles
        foreach(MoleBehavior mole in moles)
        {
            mole.stopGame();
        }
        //Stop the game and show the start UI
        playing = false;
        playButton.SetActive(true);
    }
    //Funtction to add score when a mole is clicked
    public void addScore(int moleIndex, int pointsToAdd)
    {
        //Add and update score
        score += pointsToAdd;
        scoreText.text = $"{score}";

        //Remove from active moles
        currentMoles.Remove(moles[moleIndex]);

    }

    //Function to remove score when a jester is clicked
    public void subtractScore(int moleIndex)
    {
        //Subtract and update score
        score -= 2;
        scoreText.text = $"{score}";

        //Remove jester from acctive moles since it is a mole type
        currentMoles.Remove(moles[moleIndex]);
    }

    //Function for when a mole is missed
    public void Missed(int moleIndex, bool isMole)
    {
    
        currentMoles.Remove(moles[moleIndex]);

    }
    //Update is called once per frame
    void Update()
    {
        if(playing)
        {
            //Timer code by Isiah Banes
            if(remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
            else if(remainingTime <= 0)
            {
                remainingTime = 0;
                GameOver();
                timerText.color = Color.red;
            }
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60); 
            timerText.text = string.Format("{0:00}:{1:00}", minutes , seconds);

            //Update time
            // timeRemaining -= Time.deltaTime;
            // if(timeRemaining <= 0)
            // {
            //     timeRemaining = 0;
            //     GameOver();
            // }
            //  Check if we want to activate more moles
            // More moles are activated every 10 moles hit
            if(currentMoles.Count <= (score/10))
            {
                //Choose a random mole
                int index = Random.Range(0, moles.Count);
                //If the mole is already doing something we will try to activate it again next frame
                //If statement checks if the selected mole is not activated
                if(!currentMoles.Contains(moles[index]))
                {
                    //Activates the selected mole
                    currentMoles.Add(moles[index]);
                    moles[index].Activate(score/10); //score/10 is the level of the activated mole
                }
            }
        }
    }
}
