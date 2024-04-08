using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public GameObject castButton, withdrawButton, continueButton;
    public TextMeshProUGUI fishText;
    public GameObject[] buttons;
    public Canvas canvas;

    float fishTimer = 0;
    float reelTimer = 0;
    float textTimer = 0;
    int fishLevel = 0;
    int maxButtons, currentButtons;

    enum States { 
        Inactive,
        WaitingForFish,
        FishOnHook,
        FishEscaped,
        FishCaught
    }

    States currentState = States.Inactive;

    private void Update() {
        switch (currentState) {
            case States.WaitingForFish:
                FindFish();
                break;
            case States.FishOnHook:
                ReelingFish();
                break;
            case States.FishCaught:
                FishCaught();
                break;
            case States.FishEscaped:
                FishEscaped();
                break;
            default:
                break;
        }
    }
    public void CastLine() {
        currentState = States.WaitingForFish;
        castButton.SetActive(false);
        withdrawButton.SetActive(true);
    }
    public void WithdrawLine() {
        currentState = States.Inactive;
        withdrawButton.SetActive(false);
        castButton.SetActive(true);
        //fishTimer = 0;
        //fishLevel = 0;
        ResetVariables();
    }
    public void Continue() {
        continueButton.SetActive(false);
        castButton.SetActive(true);
        fishText.gameObject.SetActive(false);
        ResetVariables();
        currentState = States.Inactive;
    }
    void FindFish() {
        if (fishTimer == 0) {
            fishTimer = (float)Random.Range(1, 30);
            //Higher chance of getting a high level fish the larger the fishTimer is
            if (fishTimer > 20) {
                if (Random.Range(0,100) < 25) {
                    fishLevel = 3;
                } else if (Random.Range(0,100) < 50) {
                    fishLevel = 2;
                } else {
                    fishLevel = 1;
                }
            } else if (fishTimer > 10) {
                if (Random.Range(0,100) < 15) {
                    fishLevel = 3;
                } else if (Random.Range(0,100) < 30) {
                    fishLevel = 2;
                } else {
                    fishLevel = 1;
                }
            } else {
                if (Random.Range(0,100) < 5) {
                    fishLevel = 3;
                } else if (Random.Range(0,100) < 10) {
                    fishLevel = 2;
                } else {
                    fishLevel = 1;
                }
            }
        }
        if (fishTimer > 0) {
            fishTimer -= Time.deltaTime;
            if (fishTimer <= 0) {
                Debug.Log("Fish Level: " + fishLevel);
                currentButtons = 0;
                maxButtons = Random.Range(7, 13);
                withdrawButton.SetActive(false);
                currentState = States.FishOnHook;
            }
        }
    }
    void ReelingFish() {
        if (reelTimer <= 0 && currentButtons < maxButtons) {
            Instantiate(buttons[Random.Range(0, 2)]).transform.SetParent(canvas.transform, false);
            currentButtons++;
            reelTimer = (float)Random.Range(2, 3);
        } else {
            reelTimer -= Time.deltaTime;
        }
    }
    void FishEscaped() {
        if (textTimer == 0) {
            fishText.text = "Fish escaped!";
            fishText.color = Color.red;
            fishText.gameObject.SetActive(true);
            textTimer += Time.deltaTime;
        } else {
            textTimer += Time.deltaTime;
            if (textTimer > 2) {
                fishText.gameObject.SetActive(false);
                castButton.SetActive(true);
                ResetVariables();
                currentState = States.Inactive;
            }
        }
    }
    void FishCaught() {
        fishText.text = "Caught a level " + fishLevel + " fish!";
        fishText.color = Color.green;
        fishText.gameObject.SetActive(true);
        continueButton.SetActive(true);
    }
    public void ButtonClicked() {
        if (currentButtons >= maxButtons) {
            Debug.Log("Current: " + currentButtons + ", Max: " + maxButtons);
            currentState = States.FishCaught;
            Debug.Log("Caught a level " + fishLevel + " fish!");
        }
    }
    public void ButtonMissed() {
        currentState = States.FishEscaped;
        Debug.Log("Fish escaped!");
    }
    void ResetVariables() {
        fishTimer = 0;
        reelTimer = 0;
        textTimer = 0;
        fishLevel = 0;
        maxButtons = 0;
        currentButtons = 0;
        fishText.gameObject.SetActive(false);
    }
}
