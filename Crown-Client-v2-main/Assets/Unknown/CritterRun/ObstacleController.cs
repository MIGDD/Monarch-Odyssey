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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > limeScooterTimer)
        {
            horizontalSpawnPoint = Random.Range(-2.5f,2.5f);
            obstaclePosition = new Vector3(horizontalSpawnPoint,0,this.transform.localPosition.z+30);
            Instantiate(limeScooter,obstaclePosition,Quaternion.identity); 
            limeScooterTimer += limeScooterSpawnInterval;
            limeScooterCount++;
        }
    }
}
