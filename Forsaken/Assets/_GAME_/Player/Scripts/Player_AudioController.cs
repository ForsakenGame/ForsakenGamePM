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
    public AudioClip machinegun;
    public AudioClip machinegunEnd;

    private List<AudioClip> _footsteps;
    private Animator _animator;
    private string _shootingAudioKey = "machinegun"; // Unique key to track sound

    private readonly List<string> shootingAnimations = new List<string>
    {
        "Anim_Player_RunningGun_Down",
        "Anim_Player_RunningGun_Right",
        "Anim_Player_RunningGun_Up",
        "Anim_Player_StaticGun_Down",
        "Anim_Player_StaticGun_Right",
        "Anim_Player_StaticGun_Up"
    };

    private void Start()
    {
        _footsteps = new List<AudioClip>
        {
            footstep1,
            footstep2,
            footstep3,
            footstep4,
            footstep5
        };

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        GatherInput();
    }

    private void GatherInput()
    {
        bool isShooting = Input.GetMouseButton(1) && IsShootingAnimationPlaying();

        if (isShooting)
        {
            PlayShootingSound();
        }
        else
        {
            StopShootingSound();
        }
    }

    public void PlayRandomFootstep()
    {
        System.Random random = new System.Random();
        int i = random.Next(1, _footsteps.Count);
        AudioClip footstep = _footsteps[i];

        AudioManager.instance.enabled = true;
        AudioManager.instance.PlayAudio(footstep, "footstep" + i, 1f, false);
    }

    public void PlayShootingSound()
    {
        if (!AudioManager.instance.IsPlaying(_shootingAudioKey)) // Prevent stacking
        {
            AudioManager.instance.PlayAudio(machinegun, _shootingAudioKey, 1f, true); // Set to loop
            // Debug.Log("Started shooting sound");
        }
    }

    public void StopShootingSound()
    {
        if (AudioManager.instance.IsPlaying(_shootingAudioKey)) // Stop if playing
        {
            AudioManager.instance.StopAudio(_shootingAudioKey);
            AudioManager.instance.PlayAudio(machinegunEnd, "machinegunEnd", 1f, false);
            // Debug.Log("Stopped shooting sound");
        }
    }

    private bool IsShootingAnimationPlaying()
    {
        if (_animator == null) return false;

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        foreach (string animationName in shootingAnimations)
        {
            if (stateInfo.IsName(animationName))
            {
                // Debug.Log("Shooting animation is playing");
                return true;
            }
        }

        // Debug.Log("quack, quack"); // Debugging, remove later
        return false;
    }
}
