using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    // --------------------------------------------------
    // ----- Singleton
    public static Character_Controller Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Character_Controller>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("Character_Controller");
                    instance = instanceContainer.AddComponent<Character_Controller>();
                }
            }
            return instance;
        }
    }
    private static Character_Controller instance;

    // ----- Component
    public Rigidbody rb;
    public Animator Anim;

    // ----- Setting
    public float moveSpeed = 5f;

    // --------------------------------------------------
    // ----- Start
    void Start()
    {
        Init();
    }

    private void Init()
    {
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
    }

    // --------------------------------------------------
    // ----- Update
    void FixedUpdate()
    {
        if (Joystick_Controller.Instance.joyVec.x != 0 || Joystick_Controller.Instance.joyVec.y != 0)
        {
            rb.velocity = new Vector3(Joystick_Controller.Instance.joyVec.x, rb.velocity.y, Joystick_Controller.Instance.joyVec.y) * moveSpeed;
            rb.rotation = Quaternion.LookRotation(new Vector3(Joystick_Controller.Instance.joyVec.x, 0, Joystick_Controller.Instance.joyVec.y));
        }
    }
}
