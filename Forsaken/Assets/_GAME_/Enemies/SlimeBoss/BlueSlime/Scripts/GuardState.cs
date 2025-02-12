using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "GuardState (S)", menuName = "ScriptableObjects/States/GuardState")]
public class GuardState : State
{
    public Vector3 guardPoint;
    public AnimationClip clip;

    public override State Run(GameObject owner)
    {
        Animator animator = owner.GetComponent<Animator>();

        animator.Play(clip.name);

        owner.GetComponent<NavMeshAgent>().SetDestination(guardPoint);

        return base.Run(owner);
    }
}
