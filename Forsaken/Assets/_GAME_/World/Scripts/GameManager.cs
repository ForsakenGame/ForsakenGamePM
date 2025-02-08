using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioClip mainMenuMusic;
    public AudioClip lobbyMusic;
    public AudioClip level1Music;

    private string currentSceneName;
    [SerializeField] GameObject pauseMenu;
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
        currentSceneName = SceneManager.GetActiveScene().name;
        AudioManager.instance.StopAudio(currentSceneName + "Music");
        SceneManager.LoadScene(sceneName);
    }

    // This function is called every time a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
         Debug.Log("Scene loaded: " + scene.name);

        // Scene checking to play the appropiate music
        if (scene.name == "Level_1")
        {
            if (AudioManager.instance != null && level1Music != null)
            {
                Debug.Log("Play this");
                AudioManager.instance.PlayAudio(level1Music, "Level_1Music", 0.01f, true);
            }
        }
        else if (scene.name == "Main_Menu")
        {
            if (AudioManager.instance != null && mainMenuMusic != null)
            {
                AudioManager.instance.PlayAudio(mainMenuMusic, "Main_MenuMusic", 0.1f, true);
            }
        }
        else if(scene.name == "Lobby")
        {
            if (AudioManager.instance != null && lobbyMusic != null)
            {
                AudioManager.instance.PlayAudio(lobbyMusic, "LobbyMusic", 0.1f, true);
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level_1");
        Time.timeScale = 1;
    }

    public void GoToMainMenuScene()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        AudioManager.instance.StopAudio(currentSceneName + "Music");
        SceneManager.LoadScene("Main_Menu");
        Time.timeScale = 1;
    }
}
