using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickButton : FishButton
{
    float xScale = 1f;
    float yScale = 1f;

    private void Update() {
        if (successfullyClicked) {
            TimedDestroy();
        } else {
            Shrink();
        }
    }

    /// <summary>
    /// Decreases the button size by a speed constant
    /// </summary>
    void Shrink() {
        xScale -= Time.deltaTime/(speedConstant - difficulty)/1.5f;
        yScale -= Time.deltaTime/(speedConstant - difficulty)/1.5f;
        transform.localScale = new Vector2(xScale, yScale);
        if (xScale <= 0) {
            fishManager.GetComponent<FishManager>().ButtonMissed();
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// Confirms the button has been clicked; Called on button click event
    /// </summary>
    public void Clicked() {
        if (!successfullyClicked) {
            GetComponent<Image>().color = Color.green;
            successfullyClicked = true;
            fishManager.GetComponent<FishManager>().ButtonClicked();
        }
    }
}
