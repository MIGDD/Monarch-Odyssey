using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiButton : FishButton
{
    public GameObject innerCircle;
    public Image timerImage;
    float xScale = 0.1f;
    float yScale = 0.1f;
    float timer = 1f;

    private void Update() {
        if (successfullyClicked || unsuccessfullyClicked) {
            TimedDestroy();
        } else {
            DecreaseTime();
        }
    }
    void DecreaseTime() {
        timer -= Time.deltaTime/((speedConstant - difficulty)*2);
        timerImage.fillAmount = timer;
        if (timer <= 0) {
            unsuccessfullyClicked = true;
            innerCircle.GetComponent<Image>().color = Color.red;
            fishManager.GetComponent<FishManager>().ButtonMissed();
        }
    }
    public void Clicked() {
        if (!successfullyClicked && !unsuccessfullyClicked) {
            xScale += 0.1f;
            yScale += 0.1f;
            innerCircle.transform.localScale = new Vector2(xScale, yScale);
            if (xScale >= 1f) {
                successfullyClicked = true;
                innerCircle.GetComponent<Image>().color = Color.green;
                fishManager.GetComponent<FishManager>().ButtonClicked();
            }
        }
    }
}
