using System.Collections;
using UnityEngine;

public class EnemyCat : MonoBehaviour
{
    RoomCondition RoomConditionGO;
    GameObject Player;

    public Transform BoltGenPosition; //발사체 생성위치
    public bool lookAtPlayer = true;
    public GameObject LaserEffect;

    float attackTime = 5f;
    float attackTimeCalc = 5f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        RoomConditionGO = transform.parent.transform.parent.gameObject.GetComponent<RoomCondition>();
        LaserEffect.SetActive(false);
        StartCoroutine(WaitPlayer());
        StartCoroutine(LaserOff());
    }

    IEnumerator LaserOff()
    {
        while (true)
        {
            yield return null;
            if (LaserEffect.activeInHierarchy)
            {
                attackTimeCalc -= Time.deltaTime;
                if (attackTimeCalc <= 0)
                {
                    attackTimeCalc = attackTime;
                    LaserEffect.SetActive(false);
                }
            }
        }
    }

    IEnumerator WaitPlayer()
    {
        yield return null;

        while (!RoomConditionGO.playerInThisRoom)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(6f);

        StartCoroutine(SetTarget());

        yield return new WaitForSeconds(1.5f);
        Shoot();

        StartCoroutine(Targeting());
    }

    IEnumerator SetTarget()
    {
        LaserEffect.SetActive(true);

        while (true)
        {
            yield return null;
            if (!lookAtPlayer) break;
            transform.LookAt(Player.transform.position);
        }
    }

    IEnumerator Targeting()
    {
        while (true)
        {
            yield return null;
            if (!LaserEffect.activeInHierarchy)
            {
                break;
            }
            Quaternion targetRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
        }
    }

    public void Shoot()
    {
        lookAtPlayer = false;
    }
}