using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Transform enemy;

    public Slider hpBar;
    public Slider backHpBar;
    public bool backHpHit = false;

    public float maxHp;
    public float currentHp;

    public GameObject HpLineFolder;
    //float unitHp = 200f;

    private void Start()
    {
        hpBar.value = currentHp / maxHp;
        backHpBar.value = hpBar.value;
    }

    void Update()
    {
        transform.position = enemy.position;
        hpBar.value = Mathf.Lerp(hpBar.value, currentHp / maxHp, Time.deltaTime * 5f);

        if ( backHpHit )
        {
            backHpBar.value = Mathf.Lerp(backHpBar.value, currentHp / maxHp, Time.deltaTime * 10f);
            if ( hpBar.value >= backHpBar.value - 0.01f )
            {
                backHpHit = false;
                backHpBar.value = hpBar.value;
            }
        }
    }

    public void Dmg()
    {
        currentHp -= 300f;
        Invoke("BackHpFun", 0.5f);
    }

    public void BackHpFun()
    {
        backHpHit = true;
    }

/*    public void GetHpBoost()
    {
        float scaleX = (1000f / unitHp) / (maxHp / unitHp);

        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }*/
}
