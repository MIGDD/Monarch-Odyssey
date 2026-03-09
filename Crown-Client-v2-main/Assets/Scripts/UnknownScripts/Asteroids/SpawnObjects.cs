using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpawnObjects : MonoBehaviour
{
    //doesn't yet have if player or enemy collects respawn objects? I might have to either add it or mess with code - Eric
    public bool done = true;
    //the game object the prefab now is called
    public GameObject PaperPrefab;
    float setup = 0;
    float funtimes;
    // private GameObject[] fakeDonuts;
    int realDonuts;
    public List<GameObject> spawnedObjects = new List<GameObject>();
    private bool canSpawn = true;


    //float setup2 = 3;
    // Update is called once per frame
    void Start()
    {
        //starts the timer memory,  puts the timer time in funtimes
        funtimes = countdowntimer.currentTime;
        // fakedonuts = new List<GameObject>();
        StartCoroutine(spawner());

    }


    private void Update()
    {
        //makes funtimes be the same as timer
        funtimes -= 1 * Time.deltaTime;
        

    }


    IEnumerator spawner()
    {
        //In collection script, make sure that it tells the spawner that the variable done is equal to true



        while (done == true)
        {



            while (done == true)
            {
                yield return new WaitForSeconds(3);
                if (!canSpawn)
                {
                    while (!canSpawn)
                    {
                        yield return null;
                    }
                }
                int spawnPointX = Random.Range(-35, 30);
                int spawnPointY = Random.Range(-3, -2);
                int spawnPointZ = Random.Range(15, 78);


                Vector3 spawnPosition = new Vector3(spawnPointX, spawnPointY, spawnPointZ);
                GameObject newDonut = Instantiate(PaperPrefab, spawnPosition, Quaternion.identity);
                spawnedObjects.Add(newDonut);

                //if paper spawns are equal to or greater than 5, stops spawning
                if (spawnedObjects.Count >= 5)
                {
                   // Debug.Log("paused spawning");
                    canSpawn = false;

                }



                //if timer is less than 10 seconds
                if (funtimes < 10)
                {
                   
                    setup = 3;
                    break;

                }
            }
            //while timer is less than 10 seconds
            while (funtimes < 10)
            {

                yield return new WaitForSeconds(2);

                if (!canSpawn)
                {
                    while (!canSpawn)
                    {
                        yield return null;
                    }
                }
                int spawnPoint2X = Random.Range(-35, 30);
                int spawnPoint2Y = Random.Range(-5, -4);
                int spawnPoint2Z = Random.Range(15, 78);



                Vector3 spawnPosition2 = new Vector3(spawnPoint2X, spawnPoint2Y, spawnPoint2Z);
                GameObject newDonut = Instantiate(PaperPrefab, spawnPosition2, Quaternion.identity);
                spawnedObjects.Add(newDonut);
                //same as before, stops spawning when its 5
                if (spawnedObjects.Count >= 5)
                {
                   // Debug.Log("paused spawning");
                    canSpawn = false;

                }

                //Eric-jensen


                if (countdowntimer.currentTime == 0)
                {
                    break;
                }
            }

        }







    }



    //it
    //function
    //call in update


}

