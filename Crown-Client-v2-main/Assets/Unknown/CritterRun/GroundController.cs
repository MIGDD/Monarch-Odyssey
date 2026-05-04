using UnityEngine;

public class GroundController : MonoBehaviour
{
    public static GroundController Instance;

    public GameObject groundPrefab;
    private Vector3 nextGroundTilePosition;
    private int groundCount = 0;
    
    void Awake()
    {
        Instance = this;

        nextGroundTilePosition = transform.position;
    }

    void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            SpawnGround();
        }
    }
    public void SpawnGround()
    {
        GameObject temp = Instantiate(groundPrefab, nextGroundTilePosition, Quaternion.identity);
        groundCount++;
        nextGroundTilePosition = temp.transform.Find("EndPoint").position;
    }
    public void DecrementGroundCount()
    {
        groundCount--;
        SpawnGround();
    }
}
