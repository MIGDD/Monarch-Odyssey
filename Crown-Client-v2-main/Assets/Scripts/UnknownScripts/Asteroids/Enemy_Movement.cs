using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


//Also includes RayCasting

public class Enemy_Movement : MonoBehaviour
{

    public float movement_speed;
    public GameObject Enemy;
    Ray ray;
    private bool collect = false; //item is collected
    private bool look = false; //if enemy is looking at something
    private bool canMove = true;//allows for enemy to 'pause' after donut
    public float Waitfor = 5f;
    private float MoveTimer = 0f;

    private List<GameObject> seenPaper = new List<GameObject>();
   
    //CanFire
    public LaunchProjectile launchProjectile;

    public SpawnObjects spawnObjects;

    //Enemy Score
    private int Enemy_score = 0;
    [SerializeField]
    Text Enemy_count;





    // Start is called before the first frame update
    void Start()
    {

        Enemy_count.text = "Enemy: " + 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        look = false;

        Rigidbody Enemy_body = Enemy.GetComponent<Rigidbody>();


        RaycastHit hit;


        //Once Timer is up, Enemy Can Move again
        if (Time.time > MoveTimer)
        {
            canMove = true;
        } 
        


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f)) //enemy collection
        {
            

            if (hit.collider.tag == "Asteroid")
            {
               
                
                collect = true;
                look = false;
                Enemy_body.constraints = RigidbodyConstraints.FreezePosition;
                //Allows another paper to spawn
                spawnObjects.spawnedObjects.RemoveAt(spawnObjects.spawnedObjects.Count - 1);
                seenPaper.RemoveAt(0);
                Destroy(hit.collider.transform.parent.gameObject);

                Enemy_score++;
                Enemy_count.text = "Enemy: " + Enemy_score.ToString();

            }

            

            else if (hit.collider.tag == "Donut")
            {
               
                Destroy(hit.collider.gameObject);
                collect = false;
                look = false;
                Enemy_body.constraints = RigidbodyConstraints.FreezePosition;
                //Start Freeze Timer
                MoveTimer = Time.time + Waitfor;
                canMove= false;
                //Player can Shoot again
                launchProjectile.canfire = true;

            }


        }



        if (canMove == true && seenPaper.Count !=0 )  //makes sure each paper is collected
        {
            if (seenPaper[0] != null)
            {
                if (look == false) //if not looking at something, turn towards item
                {
                    
                    Enemy.transform.LookAt(seenPaper[0].transform.position);
                    look = true;
                    Enemy_body.constraints = RigidbodyConstraints.FreezeRotation;
                    Enemy_body.constraints &= ~RigidbodyConstraints.FreezePosition;



                }

                if (collect == false) //if item still not collected, move foward
                {
                    Enemy_body.AddForce(transform.forward * movement_speed);



                }
            }
        }


    }

    void OnTriggerEnter(Collider other) //If something is 'seen'
    {

        var thing = other.gameObject;
        Rigidbody Enemy_body = Enemy.GetComponent<Rigidbody>();
        
        if (thing.tag == "Asteroid")
        {
            seenPaper.Insert(0, thing.gameObject);
            collect = false;
        }

        if (canMove == true)
        {


            if (thing.tag == "Donut" && thing.GetComponent<Rigidbody>().velocity.y == 0f)
            {

                if (look == false) //if not looking at something, turn towards item
                {
                    Enemy.transform.LookAt(thing.transform.position);
                    look = true;
                    Enemy_body.constraints = RigidbodyConstraints.FreezeRotation;
                    Enemy_body.constraints &= ~RigidbodyConstraints.FreezePosition;

                    Enemy_body.AddForce(transform.forward * movement_speed * 9);

                }


            }





        }



    }


}
