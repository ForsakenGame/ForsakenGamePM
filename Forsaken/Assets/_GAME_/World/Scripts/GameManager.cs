using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioClip mainMenuMusic;
    public AudioClip lobbyMusic;
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip endOfLevelMusic;
    public AudioClip endOfLevel2Music;
    public AudioClip creditsMusic;
    public bool isBossAlive;
    private string currentSceneName;

    public List<Animator> targetAnimators;
    public List<Collider2D> doorColliders;
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
        // Debug.Log("Changing scene");
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
                // Debug.Log("Play this");
                AudioManager.instance.PlayAudio(level1Music, "Level_1Music", 0.1f, true);
            }
        }
        else if (scene.name == "Level_2")
        {
            if (AudioManager.instance != null && mainMenuMusic != null)
            {
                AudioManager.instance.StopAudio("EndLevel_1Music");
                AudioManager.instance.PlayAudio(level2Music, "Level_2Music", 0.1f, true);
            }
        }
        else if (scene.name == "Credits")
        {
            if (AudioManager.instance != null && mainMenuMusic != null)
            {
                AudioManager.instance.StopAudio("EndLevel_2Music");
                AudioManager.instance.PlayAudio(creditsMusic, "CreditsMusic", 0.1f, true);
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
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        // SceneManager.LoadScene("Level_1");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void GoToMainMenuScene()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        AudioManager.instance.StopAudio(currentSceneName + "Music");
        SceneManager.LoadScene("Main_Menu");
        Time.timeScale = 1;
    }

    public void BossEnd()
    {
        FindAnimatorsAndColliders();
        if (!isBossAlive)
        {
            foreach (var targetAnimator in targetAnimators)
            {
                targetAnimator.SetTrigger("Open");
            }

            // Disable Collider After Animation Completes
            StartCoroutine(DisableColliderAfterDelay(0.1f));
            // Switch Music
            AudioManager.instance.StopAudio("BossMusic");
            if(currentSceneName == "Lobby")
            {
                AudioManager.instance.PlayAudio(endOfLevelMusic, "EndLevel_1Music", 0.1f, true);
            }
            else
            {
                //Debug.Log(currentSceneName);
                AudioManager.instance.PlayAudio(endOfLevel2Music, "EndLevel_2Music", 0.1f, true);
            }
        }
    }

    private IEnumerator DisableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var doorCollider in doorColliders)
        {
            doorCollider.enabled = false;
        }
    }

    private void FindAnimatorsAndColliders()
    {
        targetAnimators.Clear();
        doorColliders.Clear();

        // Find all animators tagged as "Door"
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Door"))
        {
            Animator animator = obj.GetComponent<Animator>();
            if (animator != null)
            {
                targetAnimators.Add(animator);
            }

            Collider2D collider = obj.GetComponent<Collider2D>();
            if (collider != null)
            {
                doorColliders.Add(collider);
            }
        }
    }
}
