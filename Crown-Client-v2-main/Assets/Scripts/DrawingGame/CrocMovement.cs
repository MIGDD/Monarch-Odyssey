using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrocMovement : MonoBehaviour
{
    NavMeshAgent nm;
    Rigidbody rb;

    public Transform Target;
    public Transform[] Square, Triangle, Star, WayPoints;
    public int Cur_Waypoint;
    public float speed, stop_distance;
    public float PauseTimer;
    private List<Transform[]> points = new List<Transform[]>();
    private Animator critterAnimator;
    private static int score;
    private int pointNum, listLength;
    [SerializeField]
    private float cur_timer;


    void Start()
    {
        pointNum = 0;
        points.Add(Square);    //Adds shapes to the be randomized
        points.Add(Triangle);
        points.Add(Star);
        
        critterAnimator = GetComponent<Animator>();

        nm = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        listLength = points.Count - 1;
        rb.freezeRotation = true;
        //Randomizes the order of the shapes to be traced out by the croc
        for (int i = 0; i <= listLength; i++)
        {
            int index = Random.Range(0, points.Count);
            for (int j = 0; j < points[index].Length; j++)
            {
                WayPoints[pointNum] = points[index][j];
                pointNum++;
            }
            points.RemoveAt(index);
        }
        Target = WayPoints[Cur_Waypoint];
        cur_timer = PauseTimer;

    }

    void Update()
    {
        score = SpawnSphere.currentScore;

        nm.acceleration = speed;
        nm.stoppingDistance = stop_distance;

        float distance = Vector3.Distance(transform.position, Target.position);

        if (distance > stop_distance && WayPoints.Length > 0)
        {
            ///play animation when moving
            critterAnimator.SetBool("isMoving", true);

            Target = WayPoints[Cur_Waypoint];
        }

        else if (distance <= stop_distance && WayPoints.Length > 0)
        {

            if (cur_timer > 0)
            {
                cur_timer -= 0.01f;
                if (cur_timer < 1)
                {
                    critterAnimator.SetBool("isMoving", false);
                }
            }

            if (cur_timer <= 0)
            {
                if (Cur_Waypoint < WayPoints.Length-1)
                {
                    Cur_Waypoint++;
                }

                Target = WayPoints[Cur_Waypoint];
                cur_timer = PauseTimer;
            }

        }

        nm.SetDestination(Target.position);
    }
}
        
       
   
