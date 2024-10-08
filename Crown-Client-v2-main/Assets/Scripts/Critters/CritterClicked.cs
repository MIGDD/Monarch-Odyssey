using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterClicked : MonoBehaviour
{
    public void OnMouseDown()
    {
        //if overhead map is on and player clicks on waypoint, display active quest details window
        Debug.Log(gameObject.name + " Clicked!");
        //GameManager.instance.ChangeScene("CritterEncounter");
        //GameManager.instance.LoadNewScene("CritterEncounter");
        GameManager.instance.LoadMiniGame("CritterEncounter");

        //SceneLoader.Load(SceneNames.CritterEncounter);

    }
}
