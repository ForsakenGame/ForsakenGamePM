using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomBehaviour : MonoBehaviour
{
    public List<Animator> targetAnimators;
    public GameObject BossHealthBar;
    public AudioClip bossMusic;
    
    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!hasPlayed && collider.CompareTag("Player"))
        {
            foreach (var targetAnimator in targetAnimators)
            {
                targetAnimator.SetTrigger("Play");
            }

            BossHealthBar.SetActive(true);

            // Play Boss Music
            AudioManager.instance.StopAudio("Level_1Music");
            AudioManager.instance.PlayAudio(bossMusic, "BossMusic", 0.1f, true);

            hasPlayed = true;
        }
    }
}
