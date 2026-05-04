using UnityEngine;

/*
Script handles player collisions,
*/
public class CritterController : MonoBehaviour
{
    private Animator critterAnimator;

    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        critterAnimator = GetComponentInChildren<Animator>();
        critterAnimator.SetBool("isMoving", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "obstacle")
        {
            player.Destroy();
            Time.timeScale = 0;
        }
    }
}
