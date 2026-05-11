using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [Header("Spawnable GameObjects")]
    public GameObject limeScooter;
    public GameObject[] staticObjects;

    [Header("Spawn Components")]
    public float limeScooterSpawnInterval = 5f;
    private int limeScooterCount = 0;
    private float limeScooterTimer = 0f;
    private float horizontalSpawnPoint;
    private Vector3 obstaclePosition;
    private Transform player;

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        if(Time.time > limeScooterTimer)
        {
            horizontalSpawnPoint = Random.Range(-2.5f,2.5f);
            // Spawn ahead of the player, not the (static) GameManager — otherwise
            // the player eventually outruns the fixed spawn z and scooters appear behind.
            obstaclePosition = new Vector3(horizontalSpawnPoint, 0, player.position.z + 30);
            Instantiate(limeScooter,obstaclePosition,Quaternion.identity);
            limeScooterTimer += limeScooterSpawnInterval;
            limeScooterCount++;
        }
    }
}
