using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetting : MonoBehaviour
{
    public static PlayerTargetting Instance // singlton     
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerTargetting>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerTargetting");
                    instance = instanceContainer.AddComponent<PlayerTargetting>();
                }
            }
            return instance;
        }
    }
    private static PlayerTargetting instance;

    public bool getATarget = false;
    float currentDist = 0;
    float closetDist = 100f;
    float TargetDist = 100f;
    int closeDistIndex = 0;
    public int TargetIndex = -1;
    int prevTargetIndex = 0;
    public LayerMask layerMask;

    public float atkSpd = 1f;

    public List<GameObject> MonsterList = new List<GameObject>(); 

    public GameObject PlayerBolt;
    public Transform AttackPoint;

    void OnDrawGizmos()
    {
        if (getATarget)
        {
            for (int i = 0; i < MonsterList.Count; i++)
            {
                if (MonsterList[i] == null) { return; }
                RaycastHit hit; //	
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position, out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawRay(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position);
            }
        }
    }

    void Update()
    {
        SetTarget();
        AtkTarget();
    }

    void Attack()
    {
        Character_Controller.Instance.Anim.SetFloat("AttackSpeed", atkSpd);
        Instantiate(PlayerBolt, AttackPoint.position, transform.rotation);
    }

    void SetTarget()
    {
        if (MonsterList.Count != 0)
        {
            prevTargetIndex = TargetIndex;
            currentDist = 0f;
            closeDistIndex = 0;
            TargetIndex = -1;

            for (int i = 0; i < MonsterList.Count; i++)
            {
                if (MonsterList[i] == null) { return; }
                currentDist = Vector3.Distance(transform.position, MonsterList[i].transform.GetChild(0).position);

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position, out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    if (TargetDist >= currentDist)
                    {
                        TargetIndex = i;

                        TargetDist = currentDist;

                        if (!Joystick_Controller.Instance.isPlayerMoving && prevTargetIndex != TargetIndex)
                        {
                            TargetIndex = prevTargetIndex;
                        }
                    }
                }

                if (closetDist >= currentDist)
                {
                    closeDistIndex = i;
                    closetDist = currentDist;
                }
            }

            if (TargetIndex == -1)
            {
                TargetIndex = closeDistIndex;
            }
            closetDist = 100f;
            TargetDist = 100f;
            getATarget = true;
        }

    }

    void AtkTarget()
    {
        if (TargetIndex == -1 || MonsterList.Count == 0)
        {
            Character_Controller.Instance.Anim.SetBool("Attack", false);
            return;
        }
        if (getATarget && !Joystick_Controller.Instance.isPlayerMoving && MonsterList.Count != 0)
        {
            transform.LookAt(MonsterList[TargetIndex].transform.GetChild(0));

            if (Character_Controller.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Character_Controller.Instance.Anim.SetBool("Idle", false);
                Character_Controller.Instance.Anim.SetBool("Walk", false);
                Character_Controller.Instance.Anim.SetBool("Attack", true);
            }

        }
        else if (Joystick_Controller.Instance.isPlayerMoving)
        {
            if (!Character_Controller.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                Character_Controller.Instance.Anim.SetBool("Attack", false);
                Character_Controller.Instance.Anim.SetBool("Idle", false);
                Character_Controller.Instance.Anim.SetBool("Walk", true);
            }
        }
        else
        {
            Character_Controller.Instance.Anim.SetBool("Attack", false);
            Character_Controller.Instance.Anim.SetBool("Idle", true);
            Character_Controller.Instance.Anim.SetBool("Walk", false);
        }
    }
}
