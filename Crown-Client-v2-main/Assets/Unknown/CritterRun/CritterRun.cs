using UnityEngine;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal")*horizontalMoveSpeed, 0f, verticalMoveSpeed);
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
