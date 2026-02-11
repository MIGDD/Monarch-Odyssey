using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StackingManager : MonoBehaviour
{
    public GameObject[] fallingObject;
    public GameObject life1, life2, life3;
    public GameObject menuScreen, playButton, continueButton, exitButton, instructionScreen, lifeGUI;

    private float randX = 0;
    private float timer = 0;
    private float timeUntilNextObject = 1;
    private int winningScore = 25;
    private int lives = 3;
    private bool loseALife = false;
    private bool gameOver = true;
    private bool startGame = false;

    public Text score;
    public TMP_Text win, lose;

    
    void Update() {
        if (!startGame && !menuScreen.activeInHierarchy) {
            startGame = true;
            gameOver = false;
        }
        if (!gameOver) {
            InstantiateObjects();
            TrackScore();
        }
        TrackLives();
    }

    private void TrackScore() {
        score.text = "Score: " + StackerMovement.instance.stacked.ToString();
        if (StackerMovement.instance.stacked == winningScore) {
            //Time.timeScale = 0;
            gameOver = true;
            win.gameObject.SetActive(true);
            instructionScreen.SetActive(false);
            playButton.SetActive(false);
            exitButton.SetActive(false);
            continueButton.SetActive(true);
            menuScreen.SetActive(true);
        }
        if (lives < 1) {
            //Time.timeScale = 0;
            gameOver = true;
            lose.gameObject.SetActive(true);
            instructionScreen.SetActive(false);
            playButton.SetActive(false);
            lifeGUI.SetActive(false);
            exitButton.SetActive(false);
            continueButton.SetActive(true);
            menuScreen.SetActive(true);
        }
    }

    /// <summary>
    /// When player loses a life, gradually remove it from the scene
    /// </summary>
    private void TrackLives() {
        if (lives < 3 && life3.activeInHierarchy) {
            float xScale = life3.transform.localScale.x - 4f * Time.deltaTime;
            float yScale = life3.transform.localScale.y - 4f * Time.deltaTime;
            life3.transform.localScale = new Vector3(xScale, yScale);
            if (xScale <= 0) {
                life3.SetActive(false);
            }
        }
        if (lives < 2 && life2.activeInHierarchy) {
            float xScale = life2.transform.localScale.x - 4f * Time.deltaTime;
            float yScale = life2.transform.localScale.y - 4f * Time.deltaTime;
            life2.transform.localScale = new Vector3(xScale, yScale);
            if (xScale <= 0) {
                life2.SetActive(false);
            }
        }
        if (lives < 1 && life1.activeInHierarchy) {
            float xScale = life1.transform.localScale.x - 4f * Time.deltaTime;
            float yScale = life1.transform.localScale.y - 4f * Time.deltaTime;
            life1.transform.localScale = new Vector3(xScale, yScale);
            if (xScale <= 0) {
                life1.SetActive(false);
            }
        }
    }

    void InstantiateObjects() {
        if (timer >= timeUntilNextObject) {
            float randObj = Random.Range(0, 2);

            randX = Random.Range(-8f, 8f); //Range Objects will fall within
            randObj = Random.Range(0f, 2f);
            Debug.Log(randObj);
            if (randObj <= 1 && randObj >= 0) randObj = 0;
            if (randObj <= 2 && randObj >= 1.1f) randObj = 1;

            Instantiate(fallingObject[(int)randObj], new Vector3(randX, 7.7f, 0.0f), Quaternion.identity);
            /*for (int i = 0; i < fallingObject.Length; i++)
            {
                fallingObject[i].GetComponent<Physics>().;
            }*/
            timeUntilNextObject = Random.Range(0.2f, 2f);
            timer = 0;
        } else {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Acorn 1(Clone)" || other.gameObject.name == "Apple 1(Clone)")
            //prevent backet from being included; layer 3 is basket layer
            if (other.gameObject.layer != 3 && !gameOver) {
                Destroy(other.gameObject);
                lives--;
            }
    }
}
