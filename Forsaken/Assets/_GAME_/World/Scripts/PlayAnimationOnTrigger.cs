using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnTrigger : MonoBehaviour
{
    public List<Animator> targetAnimators;
    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!hasPlayed && collider.CompareTag("Player")) // Ensure only the player triggers it
        {
            foreach (var targetAnimator in targetAnimators)
            {
                targetAnimator.SetTrigger("Play");
            }
            hasPlayed = true; // Ensure it only plays once
        }
    }
}

