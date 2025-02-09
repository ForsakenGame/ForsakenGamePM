using UnityEngine;
using UnityEngine.EventSystems;

public class UIPersistance : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    void Awake()
    {
        if (FindObjectsOfType<UIPersistance>().Length > 1)
        {
            Destroy(gameObject);  // Destroy duplicate UI
            return;
        }

        DontDestroyOnLoad(gameObject);  // Keep only the first UI instance
    }
}