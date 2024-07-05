using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCondition : MonoBehaviour
{
    public List<GameObject> MonsterListInRoom = new List<GameObject>();
    public bool playerInThisRoom = false;
    public bool isClearRoom = false;

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInThisRoom = true;
            PlayerTargetting.Instance.MonsterList = new List<GameObject>(MonsterListInRoom);
            Debug.Log("Enter New Room! Mob Count : " + PlayerTargetting.Instance.MonsterList.Count);

        }
        if (other.CompareTag("Monster"))
        {
            MonsterListInRoom.Add(other.transform.gameObject);
            Debug.Log(" Mob name : " + other.transform.gameObject);
        }
    }

    private void OnTriggerExit ( Collider other )
    {
        if (other.CompareTag("Player"))
        {
            playerInThisRoom = false;
            PlayerTargetting.Instance.MonsterList.Clear( );
            Debug.Log ("Player Exit!");
        }
    }
}
