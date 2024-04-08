using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishButton : MonoBehaviour
{
    protected GameObject fishManager;
    protected bool successfullyClicked = false;
    float destroyTimer = 2f;

    private void Awake() {
        SetPosition();
        fishManager = GameObject.FindGameObjectWithTag("FishManager");
    }
    void SetPosition() {
        int xPosition = Random.Range(-300, 300);
        int yPosition = Random.Range(-150, 150);
        transform.position = new Vector2(xPosition, yPosition);
    }
    protected void TimedDestroy() {
        destroyTimer -= Time.deltaTime;
        if (destroyTimer <= 0) {
            Destroy(this.gameObject);
        }
    }
}
