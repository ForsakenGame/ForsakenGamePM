using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioClip mainMenuMusic;
    public AudioClip level1Music;
    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDestroy()
    {
        // Avoids memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // This function is called every time a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log("Scene loaded: " + scene.name);

        // Scene checking to play the appropiate music
        if (scene.name == "Level_1")
        {
            if (AudioManager.instance != null && level1Music != null)
            {
                AudioManager.instance.PlayAudio(level1Music, "Level1Music", 1f, true);
            }
        }
        else if (scene.name == "Main Menu")
        {
            if (AudioManager.instance != null && mainMenuMusic != null)
            {
                AudioManager.instance.PlayAudio(mainMenuMusic, "mainMenuMusic", 1f, true);
            }
        }
    }

}
