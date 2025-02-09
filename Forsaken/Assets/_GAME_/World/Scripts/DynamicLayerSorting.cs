using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    private SpriteRenderer pillarRenderer;
    public Transform player; // Assign the player in the Inspector
    private SpriteRenderer playerRenderer;

    void Start()
    {
        pillarRenderer = GetComponent<SpriteRenderer>();
        playerRenderer = player.GetComponent<SpriteRenderer>(); // Get the player's renderer
    }

    void Update()
    {
        if (player != null)
        {
            // Compare world positions instead of local Y values
            if (player.position.y < transform.position.y)
            {
                pillarRenderer.sortingOrder = 1; // Behind player
            }
            else
            {
                pillarRenderer.sortingOrder = 6; // In front of player
            }
        }
    }
}
