using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick_Controller : MonoBehaviour
{
    // --------------------------------------------------
    // ----- Singleton
    public static Joystick_Controller Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Joystick_Controller>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("Joystick_Controller");
                    instance = instanceContainer.AddComponent<Joystick_Controller>();
                }
            }
            return instance;
        }
    }
    private static Joystick_Controller instance;

    // ----- Component

    // ----- Setting
    public GameObject smallStick;
    public GameObject bGStick;
    Vector3 stickFirstPosition;
    public Vector3 joyVec;
    Vector3 joyStickFirstPosition;
    float stickRadius;

    public bool isPlayerMoving = false;

    // --------------------------------------------------
    // ----- Start
    void Start()
    {
        stickRadius = bGStick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        joyStickFirstPosition = bGStick.transform.position;
    }


    // --------------------------------------------------
    // ----- Event
    public void PointDown()
    {
        bGStick.transform.position = Input.mousePosition;
        smallStick.transform.position = Input.mousePosition;
        stickFirstPosition = Input.mousePosition;

        if (!Character_Controller.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            Character_Controller.Instance.Anim.SetBool("Attack", false);
            Character_Controller.Instance.Anim.SetBool("Idle", false);
            Character_Controller.Instance.Anim.SetBool("Walk", true);
        }

        isPlayerMoving = true;
        PlayerTargetting.Instance.getATarget = false;
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 DragPosition = pointerEventData.position;
        joyVec = (DragPosition - stickFirstPosition).normalized;

        float stickDistance = Vector3.Distance(DragPosition, stickFirstPosition);

        if (stickDistance < stickRadius)
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickDistance;
        }
        else
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickRadius;
        }
    }

    public void Drop() 
    {
        joyVec = Vector3.zero;
        bGStick.transform.position = joyStickFirstPosition;
        smallStick.transform.position = joyStickFirstPosition;

        if (!Character_Controller.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Character_Controller.Instance.Anim.SetBool("Attack", false);
            Character_Controller.Instance.Anim.SetBool("Walk", false);
            Character_Controller.Instance.Anim.SetBool("Idle", true);
        }

        isPlayerMoving = false;
    }
}
