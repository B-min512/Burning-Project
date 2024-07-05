using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    // --------------------------------------------------
    // ----- Singleton
    public static Camera_Controller Instance    
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Camera_Controller>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("Camera_Controller");
                    instance = instanceContainer.AddComponent<Camera_Controller>();
                }
            }
            return instance;
        }
    }
    private static Camera_Controller instance;

    // ----- Setting
    public GameObject Player;

    public float offsetY = 45f;
    public float offsetZ = -40f;

    Vector3 cameraPosition;

    // --------------------------------------------------
    // ----- Start
    void Start()
    {
        
    }

    // --------------------------------------------------
    // ----- Update
    void LateUpdate()
    {
        cameraPosition.x = Player.transform.position.x;
        cameraPosition.y = Player.transform.position.y + offsetY;
        cameraPosition.z = Player.transform.position.z + offsetZ;

        transform.position = cameraPosition;
    }
}
