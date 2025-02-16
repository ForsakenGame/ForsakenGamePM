using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        GameManager.instance.ChangeScene(sceneName);
    }

    public void ResumeGame()
    {
        GameManager.instance.ResumeGame();
    }

    public void RestartGame()
    { 
        GameManager.instance.RestartGame();
    }

    public void GoToMainMenu()
    {
        GameManager.instance.GoToMainMenuScene();
    }
}
