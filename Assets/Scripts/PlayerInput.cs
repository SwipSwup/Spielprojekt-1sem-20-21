﻿using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Range(0f, 1000f)]
    [Tooltip("Distance after wich swipe is detected")]
    public float swipeDetection = 50f;

    [Range(0f, 100f)]
    [Tooltip("Distance up to wich it still counts as a tab")]
    public float tabDistance = 10f;

    private bool inputValid = false;
    private Touch touch;
    private Vector2 startTouch;
    private Vector2 deltaPosition;

    void Update()
    {
        ManageTouchInput();
        ManageKeyboardInput();
    }

    private void ManageTouchInput()
    {
        if (Input.touches.Length < 1)
        {
            inputValid = false;
            return;
        }
        touch = Input.touches[0];

        if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
        {
            if (deltaPosition.x < tabDistance && deltaPosition.x > -tabDistance &&
                deltaPosition.y < tabDistance && deltaPosition.y > -tabDistance &&
                inputValid) OnScreenTab?.Invoke();

            inputValid = false;
            return;
        }

        if (touch.phase == TouchPhase.Began)
        {
            startTouch = touch.position;
            inputValid = true;
            return;
        }

        if (touch.phase == TouchPhase.Moved && inputValid)
        {
            deltaPosition = touch.position - startTouch;

            if (deltaPosition.x < -swipeDetection)
            {
                OnSwipeLeft?.Invoke();
                inputValid = false;
                return;
            }

            if (deltaPosition.x > swipeDetection)
            {
                OnSwipeRight?.Invoke();
                inputValid = false;
                return;
            }

            if (deltaPosition.y > swipeDetection)
            {
                OnSwipeUp?.Invoke();
                inputValid = false;
                return;
            }

            if (deltaPosition.y < -swipeDetection)
            {
                OnSwipeDown?.Invoke();
                inputValid = false;
                return;
            }
        }
    }

    private void ManageKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) OnSwipeLeft?.Invoke();
        if (Input.GetKeyDown(KeyCode.RightArrow)) OnSwipeRight?.Invoke();

        if (Input.GetKeyDown(KeyCode.UpArrow)) OnSwipeUp?.Invoke();

        if (Input.GetKeyDown(KeyCode.DownArrow)) OnSwipeDown?.Invoke();
    }

    public static Action OnSwipeUp;
    public static Action OnSwipeDown;
    public static Action OnSwipeRight;
    public static Action OnSwipeLeft;

    public static Action OnScreenTab;
}