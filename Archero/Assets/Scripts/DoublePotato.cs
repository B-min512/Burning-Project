using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    public static UiController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UiController>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("UiController");
                    instance = instanceContainer.AddComponent<UiController>();
                }
            }
            return instance;
        }
    }
    private static UiController instance;

    public GameObject JoyStickGO;
    public GameObject JoyStickPanelGO;
    public GameObject SlotMachineGO;
    public GameObject RouletteGO;
    public GameObject EndGameGO;

    public Text ClearRoomCnt;

    public Slider PlayerExpBar;
    public Slider BossHpBar;
    public Slider BossBackHpSlider;
    public bool backHpHit = false;
    public bool bossRoom = false;

    public Text playerLvText;

    public float BossCurrentHp;
    public float BossMaxHp;

    void Start()
    {
        //PlayerExpBar.value = PlayerData.Instance.PlayerCurrentExp / PlayerData.Instance.PlayerLvUpExp;
        PlayerExpBar.gameObject.SetActive(true);
        BossHpBar.gameObject.SetActive(false);
        BossBackHpSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!bossRoom)
        {
            //PlayerExpBar.value = Mathf.Lerp(PlayerExpBar.value, PlayerData.Instance.PlayerCurrentExp / PlayerData.Instance.PlayerLvUpExp, 0.75f);
            //playerLvText.text = "Lv." + PlayerData.Instance.PlayerLv;
        }
        else
        {

            BossHpBar.value = Mathf.Lerp(BossHpBar.value, BossCurrentHp / BossMaxHp, Time.deltaTime * 5f);

            if (backHpHit)
            {
                BossBackHpSlider.value = Mathf.Lerp(BossBackHpSlider.value, BossHpBar.value, Time.deltaTime * 10f);
                if (BossHpBar.value >= BossBackHpSlider.value - 0.01f)
                {
                    backHpHit = false;
                    BossBackHpSlider.value = BossHpBar.value;
                }
            }
        }
    }
    public void Dmg()
    {
        Invoke("BackHpFun", 0.5f);
    }
    void BackHpFun()
    {
        backHpHit = true;
    }

    public void CheckBossRoom(bool isBossRoom)
    {
        bossRoom = isBossRoom;

        if (isBossRoom)
        {
            PlayerExpBar.gameObject.SetActive(false);
            BossHpBar.gameObject.SetActive(true);
            BossBackHpSlider.gameObject.SetActive(true);
        }
        else
        {
            PlayerExpBar.gameObject.SetActive(true);
            BossHpBar.gameObject.SetActive(false);
            BossBackHpSlider.gameObject.SetActive(false);
        }
    }

    public void PlayerLvUp(bool isSlotMachineOn)
    {
        if (isSlotMachineOn)
        {
            JoyStickGO.SetActive(false);
            JoyStickPanelGO.SetActive(false);
            SlotMachineGO.SetActive(true);
        }
        else
        {
            JoyStickGO.SetActive(true);
            JoyStickPanelGO.SetActive(true);
            SlotMachineGO.SetActive(false);
        }
    }

    public void EndGame()
    {
        JoyStickGO.gameObject.SetActive(false);
        JoyStickPanelGO.gameObject.SetActive(false);
        StartCoroutine(EndGamePopUp());
    }

    IEnumerator EndGamePopUp()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        EndGameGO.SetActive(true);
        ClearRoomCnt.text = "Clear " + (StageManager.Instance.currentStage - 1);
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}