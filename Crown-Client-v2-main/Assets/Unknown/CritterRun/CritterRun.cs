using UnityEngine;
using TMPro;

/*
Script creates 'Endless Runner' style movement by moving the player transform.
Intened for the CritterRun mini-game
*/
public class CritterRun : MonoBehaviour
{

    [Header("Game Speed")]
    public float moveSpeed = 3f;
    public float verticalMoveSpeed = 1.25f;
    public float horizontalMoveSpeed = 1.25f;

    [Header("Speed Scaling")]
    public float speedAcceleration = 0.15f;
    public float maxMoveSpeed = 10f;

    [Header("UI")]
    public TMP_Text timerText;

    private float baseMoveSpeed;
    private float elapsedTime;

    void Start()
    {
        baseMoveSpeed = moveSpeed;
        elapsedTime = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        moveSpeed = Mathf.Min(baseMoveSpeed + speedAcceleration * elapsedTime, maxMoveSpeed);

        // moveSpeed scales only forward (z); horizontal strafe stays constant.
        Vector3 movement = new Vector3(
            Input.GetAxis("Horizontal") * horizontalMoveSpeed,0f,verticalMoveSpeed * moveSpeed
        );
        transform.position += movement * Time.deltaTime;

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText == null) return;
        int minutes = (int)(elapsedTime / 60f);
        int seconds = (int)(elapsedTime % 60f);
        int centiseconds = (int)((elapsedTime * 100f) % 100f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centiseconds);
    }
}
