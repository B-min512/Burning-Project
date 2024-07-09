using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeFSM : MonoBehaviour
{
    public enum State
    {
        Idle, Move, Attack
    };

    public State currentState = State.Idle;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
