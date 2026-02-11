using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item3DViewer : MonoBehaviour, IDragHandler
{

    public Transform Building;
 

    public void OnDrag(PointerEventData eventData){
        Building.eulerAngles += new Vector3(-eventData.delta.y, -eventData.delta.x);
    }
}
