using System.Collections;
using UnityEngine;

public class OldManScript : MonoBehaviour
{
    private bool isPlayerInRange;
    public GameObject dialogueMark;

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) 
        {
            isPlayerInRange = true;
            dialogueMark.SetActive(true);
            
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueMark.SetActive(false);
        }
    }
}
