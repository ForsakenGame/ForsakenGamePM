using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioClip level1Music;
    void Awake()
    {
        // Si la instancia es null
        if (!instance)
        {
            instance = this; // asignamos la clase GameManager (en la que estamos)
            DontDestroyOnLoad(gameObject); // gameObject es la escena actual
        }
        // Si ya hay una instancia de GameManager
        else
        {
            // Destruimos la escena
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Level_1")
        {
            Debug.Log(SceneManager.GetActiveScene().name);
            if (level1Music != null && AudioManager.instance != null)
            {
                Debug.Log("Playing music");
                AudioManager.instance.PlayAudio(level1Music, "Level1Music", 1f, true);
            }
        }
    }

}
