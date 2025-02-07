using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AudioController : MonoBehaviour
{
    public AudioClip footstep1;
    public AudioClip footstep2;
    public AudioClip footstep3;
    public AudioClip footstep4;
    public AudioClip footstep5;

    private List<AudioClip> footsteps;

    private void Start()
    {
        footsteps = new List<AudioClip>
        {
            footstep1,
            footstep2,
            footstep3,
            footstep4,
            footstep5
        };
    }

    public void PlayRandomFootstep()
    {
        System.Random random = new System.Random();
        int i = random.Next(1, footsteps.Count); // Generate a random number to choose a random footstep each time
        AudioClip footstep = footsteps[i]; // Retrieve random footstep
        AudioManager.instance.enabled = true;
        AudioManager.instance.PlayAudio(footstep, "footstep" + i, 0.1f, false);
    }
}
