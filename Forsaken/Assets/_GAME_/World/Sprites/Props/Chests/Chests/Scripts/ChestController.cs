using UnityEngine;

public class ChestController : MonoBehaviour
{
    private Animator animator;
    private bool isPlayerNearby = false;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontró el Animator en " + gameObject.name);
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpen = true;
        Debug.Log("Cofre abierto");
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
        else
        {
            Debug.LogError("No se encontró el Animator al intentar abrir el cofre.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Algo entró en el trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("¡El jugador está cerca del cofre!");
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador se alejó del cofre.");
            isPlayerNearby = false;
        }
    }
}
