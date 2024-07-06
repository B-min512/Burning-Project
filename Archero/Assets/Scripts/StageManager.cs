using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance   
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageManager>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("StageManager");
                    instance = instanceContainer.AddComponent<StageManager>();
                }
            }
            return instance;
        }
    }
    private static StageManager instance;

    public GameObject Player;

    [System.Serializable]
    public class StartPositionArray
    {
        public List<Transform> StartPosition = new List<Transform>();
    }

    public StartPositionArray[] startPositionArrays;


    public List<Transform> StartPositionAngel = new List<Transform>();

    public List<Transform> StartPositionBoss = new List<Transform>();

    public Transform StartPositionLastBoss;


    public int currentStage = 0;
    int LastStage = 20;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void NextStage()
    {
        currentStage++;
        if (currentStage > LastStage)
        { return; }

        if (currentStage % 5 != 0)
        {
            int arrayIndex = currentStage / 10;
            int randomIndex = Random.Range(0, startPositionArrays[arrayIndex].StartPosition.Count);
            Player.transform.position = startPositionArrays[arrayIndex].StartPosition[randomIndex].position;
            startPositionArrays[arrayIndex].StartPosition.RemoveAt(randomIndex);
        }
        else
        {
            if (currentStage % 10 == 5)
            {
                int randomIndex = Random.Range(0, StartPositionAngel.Count);
                Player.transform.position = StartPositionAngel[randomIndex].position;
            }
            else
            {
                if (currentStage == LastStage)
                {
                    Player.transform.position = StartPositionLastBoss.position;
                }
                else
                {
                    int randomIndex = Random.Range(0, StartPositionBoss.Count);
                    Player.transform.position = StartPositionBoss[randomIndex].position;
                    StartPositionBoss.RemoveAt(currentStage / 10);
                }
            }
        }
        Camera_Controller.Instance.CameraNextRoom();
    }
}
