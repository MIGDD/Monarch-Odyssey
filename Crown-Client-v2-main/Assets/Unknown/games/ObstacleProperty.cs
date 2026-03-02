using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleProperty : MonoBehaviour
{
    public GameObject killer,player,plat;
    public GameObject[] spawnObjects;
    private float nextActionTime = 0f;
    private float platSpawn = 0f;
    private float killerTime = 0f;
    public float period = .2f;
    public float killerInterval = 5f;
    public Vector3 obstaclePosition,platPos;
    private float spawn;
    private int spawnNum;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
       if(Time.time > nextActionTime){
            spawnNum = Random.Range(0,spawnObjects.Length);
            spawn = Random.Range(-2.5f,2f);
            obstaclePosition = new Vector3(spawn,0,this.transform.localPosition.z+30);
           nextActionTime += period;
           Instantiate(spawnObjects[spawnNum],obstaclePosition,Quaternion.identity);    
       }
       if(Time.time > killerTime)
       {
            spawn = Random.Range(-2.5f,2f);
            obstaclePosition = new Vector3(spawn,0,this.transform.localPosition.z+30);
            Instantiate(killer,obstaclePosition,Quaternion.identity); 
            killerTime += killerInterval;   

       }
      
       if(Time.time > platSpawn){
             platPos = new Vector3(0,0,this.transform.localPosition.z+35);
           platSpawn += 4;
           Instantiate(plat,platPos,Quaternion.identity);
           
       }
    }
   void OnCollisionEnter(Collision other)
		{
			if(other.gameObject.tag == "object hurt"){
				player.Destroy();
                Time.timeScale = 0;
			}
		}
}
