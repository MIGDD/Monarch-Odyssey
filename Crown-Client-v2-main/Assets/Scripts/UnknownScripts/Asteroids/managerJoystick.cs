using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class managerJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    private Image imgJoystickBg;
    private Image imgJoystick;
    private Vector2 posInput;

    // above is the images
    void Start()
    {
        //grabbing the images
        imgJoystickBg = GetComponent<Image>();
        imgJoystick = transform.GetChild(0).GetComponent<Image>();


    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgJoystickBg.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out posInput))
            //asking for the transform of bg, second asking for data, third camera where event occurs, drag cords are fourth, stored in variable
            
        {
            posInput.x = posInput.x / (imgJoystickBg.rectTransform.sizeDelta.x);
            posInput.y = posInput.y / (imgJoystickBg.rectTransform.sizeDelta.y);
            //limits the speed
            Debug.Log(posInput.x.ToString() + "/" + posInput.y.ToString());

            //normalizing values
            if (posInput.magnitude > 1.0f)
            {
                posInput = posInput.normalized;
            }


            //joystick move
            imgJoystick.rectTransform.anchoredPosition = new Vector2(
                posInput.x * (imgJoystickBg.rectTransform.sizeDelta.x) / 4, 
                posInput.y * (imgJoystickBg.rectTransform.sizeDelta.y) / 4);
        }

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        //when touch starts, switches to drag function
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        posInput = Vector2.zero;
        //character wont move once touch is over
        imgJoystick.rectTransform.anchoredPosition = Vector2.zero;
        //brings small square to center
    }
    

    //moving character

    public float inputHorizontal()
    {
        if (posInput.x != 0)
            return posInput.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float inputVertical()
    {

        if (posInput.y != 0)
            return posInput.y;
        else
            return Input.GetAxis("Vertical");

    }


}
