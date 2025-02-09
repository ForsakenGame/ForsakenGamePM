using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackState (S)", menuName = "ScriptableObjects/States/AttackState")]
public class AttackState : State
{

    public AnimationClip clip;
    public override State Run(GameObject owner)
    {

        Animator animator = owner.GetComponent<Animator>();

        animator.Play(clip.name);


        return base.Run(owner);
    }
}
