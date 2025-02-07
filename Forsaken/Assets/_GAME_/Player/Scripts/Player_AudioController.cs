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

    private bool _isRightButtonHeld;
    private AudioSource _shootingAudioSource;
    private List<AudioClip> _footsteps;
    private Animator _animator;

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

        // AudioSource for shooting
        _shootingAudioSource = gameObject.AddComponent<AudioSource>();
        _shootingAudioSource.clip = machinegun;
        _shootingAudioSource.loop = true; // Make it loop
        _shootingAudioSource.playOnAwake = false;
        _shootingAudioSource.volume = 0.1f;

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        GatherInput();
    }

    private void GatherInput()
    {
        // Avoids shooting sound playing when right clicking on incorrect animation 
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
        AudioManager.instance.PlayAudio(footstep, "footstep" + i, 0.1f, false);
    }

    public void PlayShootingSound()
    {
        if (!_shootingAudioSource.isPlaying) // Avoid audio stacking
        {
            _shootingAudioSource.Play();
        }
    }

    public void StopShootingSound()
    {
        if (_shootingAudioSource.isPlaying)
        {
            _shootingAudioSource.Stop();
        }
    }

    private bool IsShootingAnimationPlaying()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        foreach (string animationName in shootingAnimations)
        {
            if (stateInfo.IsName(animationName))
            {
                // Debug.Log("shooting animation is playing");
                // Debug.Log(stateInfo.GetHashCode());
                return true;
            }
        }
        // Debug.Log("quack, quack");
        return false;
    }
}
