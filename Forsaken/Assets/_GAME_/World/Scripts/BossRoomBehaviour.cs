using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomBehaviour : MonoBehaviour
{
    public List<Animator> targetAnimators;
    public GameObject BossHealthBar;
    public AudioClip bossMusic;
    
    private bool hasPlayed = false;
    public List<Collider2D> doorColliders;

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
            AudioManager.instance.PlayAudio(bossMusic, "BossMusic", 0.01f, true);

            hasPlayed = true;
        }
    }

    public void BossEnd()
    {
        BossHealthBar.SetActive(false);

        foreach (var targetAnimator in targetAnimators)
        {
            targetAnimator.SetTrigger("Open");
        }

        // Disable Collider After Animation Completes
        StartCoroutine(DisableColliderAfterDelay(0.1f));        
    }

    private IEnumerator DisableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var doorCollider in doorColliders)
        {
            doorCollider.enabled = false;
        }
    }
}
