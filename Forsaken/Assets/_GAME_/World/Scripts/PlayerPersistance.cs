using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPersistance : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsOfType<PlayerPersistance>().Length > 1)
        {
            Destroy(gameObject);  
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
