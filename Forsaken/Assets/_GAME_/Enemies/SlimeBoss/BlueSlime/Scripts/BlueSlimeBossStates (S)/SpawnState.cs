using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "SpawnState (S)", menuName = "ScriptableObjects/States/SpawnState")]
public class SpawnState : State
{
    public AnimationClip clip;

    public override State Run(GameObject owner)
    {
    

        Animator animator = owner.GetComponent<Animator>();

        animator.Play(clip.name);


        return base.Run(owner);
    }

}
