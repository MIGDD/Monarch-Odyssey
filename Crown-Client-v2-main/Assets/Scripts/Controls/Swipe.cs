using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{

    private bool tap;
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;
    private bool isDragging = false;

    public float swipeMagnitude = 100;

    private Vector2 startTouch;
    private Vector2 swipeDelta;


    
    // Update is called once per frame
    private void Update()
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        #region StandAlone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDragging = true;
            startTouch = Input.mousePosition;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            Reset();
        }

        #endregion

        #region Mobile Inputs

        
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDragging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDragging = false;
                Reset();
            }
        }
        

        #endregion

        //calculate the distance
        swipeDelta = Vector2.zero;
        if (isDragging)
        {
            if (Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        //did swipe move out of dead zone?

        if (SwipeDelta.magnitude > swipeMagnitude)
        {
            //which direction was swipe?
            float _x = swipeDelta.x;
            float _y = swipeDelta.y;

            if (Mathf.Abs(_x) > Mathf.Abs(_y))
            {
                if (_x < 0)
                {
                    swipeLeft = true;
                } else
                {
                    swipeRight = true;
                }
            } else
            {
                if (_y < 0)
                {
                    swipeDown = true;
                }
                else
                {
                    swipeUp = true;
                }

            }

            Reset();

        }
    }

    private void Reset()
    {
        startTouch = Vector2.zero;
        swipeDelta = Vector2.zero;
        isDragging = false;
    }

    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }




}
