using UnityEngine;

public class GroundTile : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            Time.timeScale = 0;
        }
    }
    void OnBecameInvisible(){
        Destroy(gameObject);
        GroundController.Instance.DecrementGroundCount();
    }
}
