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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("NextRoom"))
        {
            Debug.Log(" Get Next Room ");
            StageManager.Instance.NextStage();
        }

        if (other.transform.CompareTag("MeleeAtk"))
        {
            other.transform.parent.GetComponent<EnemyDuck>().meleeAtkArea.SetActive(false);
            //PlayerHpBar.Instance.currentHp -= other.transform.parent.GetComponent<EnemyDuck>().damage * 2f;

            if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Dmg"))
            {
                Anim.SetTrigger("Damaged");
                Instantiate(EffectSet.instance.playerDmgEffect, PlayerTargetting.Instance.AttackPoint.position, Quaternion.Euler(90, 0, 0));
            }
        }
    }
}
