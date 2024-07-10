using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeFSM : EnemyBase
{
    public enum State
    {
        Idle, Move, Attack
    };

    public State currentState = State.Idle;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        parentRoom = transform.parent.transform.parent.gameObject;
        Debug.Log("Start - State : " + currentState.ToString());

        StartCoroutine(FSM());
    }

    protected virtual void InitMonster() { }

    protected virtual IEnumerator FSM()
    {
        yield return null;

        while ( !parentRoom.GetComponent<RoomCondition>().playerInThisRoom )
        {
            yield return Delay500;
        }

        InitMonster();

        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    protected virtual IEnumerator Idle()
    {
        yield return null;

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            anim.SetTrigger("Idle");
        }

        if (CanAtkStateFun())
        {
            if (canAtk)
            {
                currentState = State.Attack;
            }
            else
            {
                currentState = State.Idle;
                transform.LookAt(player.transform.position);
            }
        } else
        {
            currentState = State.Move;
        }
    }

    protected virtual void AtkEffect() { }

    protected virtual IEnumerator Attack()
    {
        yield return null;

        nma.stoppingDistance = 0f;
        nma.isStopped = true;
        nma.SetDestination(player.transform.position);
        yield return Delay500;

        nma.isStopped = false;
        nma.speed = 30f;
        canAtk = false;

        if ( !anim.GetCurrentAnimatorStateInfo(0).IsName("stun"))
        {
            anim.SetTrigger("Attack");
        }
        AtkEffect();
        yield return Delay500;

        nma.speed = moveSpeed;
        nma.stoppingDistance = attackRange;
        currentState = State.Idle;
    }

    protected virtual IEnumerator Move()
    {
        yield return null;

        if ( !anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            anim.SetTrigger("Walk");
        }

        if (CanAtkStateFun() && canAtk)
        {
            currentState = State.Attack;
        } else if (distance > playerRealizeRange)
        {
            nma.SetDestination(transform.parent.position - Vector3.forward * 5f);
        } else
        {
            nma.SetDestination(player.transform.position);
        }
    }

    // Update is called once per frame
    new void Update()
    {
        
    }
}
