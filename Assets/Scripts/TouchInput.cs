using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// utility class. Helps to accept multitouch input
/// </summary>
public class TouchInput : MonoBehaviour
{
    public bool isActive;
    public LayerMask touchInputMask;

    public List<GameObject> touchList = new List<GameObject>();
    public GameObject[] touchesOld;
    public RaycastHit hit;
    private new Camera camera;

    public bool IsActive
    {
        get { return isActive; }
    }

    void Awake()
    {
        camera = Camera.main;
    }

    /// <summary>
    /// Checks single and/or multiple inputs and sends the events to the receiver. Due to the non existing property
    /// Input.touchCount on regular computers (with no touch input) the second part of the method has to be
    /// commented out for regular computer builds, as well as the if statement (#if UNITY_EDITOR // #endif). 
    /// Otherwise the touchinput won't work. For touchtable builds the if statement has to work and the second part
    /// of the code must be working too.
    /// </summary>
    void Update()
    {
        if (isActive)
        {
//#if UNITY_EDITOR

            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {
                touchesOld = new GameObject[touchList.Count];
                touchList.CopyTo(touchesOld);
                touchList.Clear();

                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchInputMask))
                {
                    GameObject recipient = hit.transform.gameObject;
                    touchList.Add(recipient);
                    Debug.Log(Input.mousePosition + " = mouse.position");

                    if (Input.GetMouseButtonDown(0))
                    {
                        recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetMouseButton(0))
                    {
                        recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }

                foreach (GameObject g in touchesOld)
                {
                    if (!touchList.Contains(g))
                    {
                        g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }

//#endif

            //if (Input.touchCount > 0)
            //{
            //    touchesOld = new GameObject[touchList.Count];
            //    touchList.CopyTo(touchesOld);
            //    touchList.Clear();

            //    foreach (Touch touch in Input.touches)
            //    {
            //        Ray ray = camera.ScreenPointToRay(touch.position);
            //        RaycastHit hit;
            //        Debug.Log(touch.position + " = touch.position");

            //        if (Physics.Raycast(ray, out hit, touchInputMask))
            //        {
            //            GameObject recipient = hit.transform.gameObject;
            //            touchList.Add(recipient);

            //            if (touch.phase == TouchPhase.Began)
            //            {
            //                recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
            //            }
            //            if (touch.phase == TouchPhase.Ended)
            //            {
            //                recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
            //            }
            //            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            //            {
            //                recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
            //            }
            //            if (touch.phase == TouchPhase.Canceled)
            //            {
            //                recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
            //            }
            //        }

            //    }

            //    foreach (GameObject g in touchesOld)
            //    {
            //        if (!touchList.Contains(g))
            //        {
            //            g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
            //        }
            //    }
            //}
        }
    }

    /// <summary>
    /// activates the touchinput. Is deactivated when menus are visible.
    /// </summary>
    /// <param name="value"></param>
    public void SetActive(bool value)
    {
        isActive = value;
    }
}