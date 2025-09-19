using UnityEngine;

public class MobileControlsManager : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;

    public float minSwipeDistance = 50f; // Minimum pixels to count as a swipe

    InputSytemActions inputActions;

    private void Awake()
    {
        inputActions = new InputSytemActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        //HandleMouse();
    }

    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isSwiping = true;
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            endTouchPosition = (Vector2)Input.mousePosition;
            DetectSwipe();
            isSwiping = false;
        }
    }

    private void DetectSwipe()
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;

        if (swipeDelta.magnitude < minSwipeDistance)
            return; // too short, not a swipe

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            // Horizontal swipe
            if (swipeDelta.x > 0)
                OnSwipeRight();
            else
                OnSwipeLeft();
        }
        else
        {
            // Vertical swipe
            if (swipeDelta.y > 0)
                OnSwipeUp();
            else
                OnSwipeDown();
        }
    }

    private void OnSwipeLeft()
    {
        Debug.Log("Swipe Left");
        // e.g. move player left
    }

    private void OnSwipeRight()
    {
        Debug.Log("Swipe Right");
        // e.g. move player right
    }

    private void OnSwipeUp()
    {
        Debug.Log("Swipe Up");
        // e.g. jump
    }

    private void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
        // e.g. slide
    }
}
