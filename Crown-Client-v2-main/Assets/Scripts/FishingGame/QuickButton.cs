using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickButton : FishButton
{
    float xScale = 1;
    float yScale = 1;

    private void Update() {
        if (successfullyClicked) {
            TimedDestroy();
        } else {
            Shrink();
        }
    }

    void Shrink() {
        xScale -= Time.deltaTime/2;
        yScale -= Time.deltaTime/2;
        transform.localScale = new Vector2(xScale, yScale);
        if (xScale <= 0) {
            fishManager.GetComponent<FishManager>().ButtonMissed();
            Destroy(this.gameObject);
        }
    }
    public void Clicked() {
        GetComponent<Image>().color = Color.green;
        successfullyClicked = true;
        fishManager.GetComponent<FishManager>().ButtonClicked();
    }
}
