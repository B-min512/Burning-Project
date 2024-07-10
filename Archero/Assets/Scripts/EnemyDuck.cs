using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyDuck : EnemyMeleeFSM
{
    public GameObject enemyCanvasGO;
    public GameObject meleeAtkArea;

    protected override void InitMonster()
    {
        base.InitMonster();

        maxHp += (StageManager.Instance.currentStage + 1) * 100f;
        currentHp = maxHp;
        damage += (StageManager.Instance.currentStage + 1) * 100f;
    }

    protected override void AtkEffect()
    {
        base.AtkEffect();
        Instantiate(EffectSet.instance.duckAtkEffect, transform.position, Quaternion.Euler(90, 0, 0));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerRealizeRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        attackCoolTime = 2f;
        attackCoolTimeCacl = attackCoolTime;

        attackRange = 3f;

        nma = GetComponent<NavMeshAgent>();
        nma.stoppingDistance = 2f;

        StartCoroutine(ResetAtkArea());
    }

    IEnumerator ResetAtkArea()
    {
        while (true)
        {
            yield return null;
            if (!meleeAtkArea.activeInHierarchy && currentState == State.Attack)
            {
                yield return new WaitForSeconds(attackCoolTime);
                meleeAtkArea.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    new void Update()
    {
        nma.SetDestination(player.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Potato"))
        {
            enemyCanvasGO.GetComponent<EnemyHpBar>().Dmg();
            currentHp -= 250f;
            Instantiate(EffectSet.instance.duckDmgEffect, collision.contacts[0].point, Quaternion.Euler(90, 0, 0));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Potato"))
        {
            enemyCanvasGO.GetComponent<EnemyHpBar>().Dmg();
            currentHp -= 250f;
            Instantiate(EffectSet.instance.duckDmgEffect, other.transform.position, Quaternion.Euler(90, 0, 0));
        }
    }
}
