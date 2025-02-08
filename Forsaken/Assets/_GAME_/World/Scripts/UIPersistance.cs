using UnityEngine;

public class UIPersistance : MonoBehaviour
{
    public GameObject menu;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);  // Make sure the UI persists across scenes
    }
}