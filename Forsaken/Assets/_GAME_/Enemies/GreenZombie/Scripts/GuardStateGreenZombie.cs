using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "GuardStateSZ (S)", menuName = "ScriptableObjects/States/GuardStateSZ")]
public class GuardStateGreenZombie : State
{
    public Vector3 guardPoint;
    public AnimationClip clip;

    public override State Run(GameObject owner)
    {
        Animator animator = owner.GetComponent<Animator>();
        NavMeshAgent agent = owner.GetComponent<NavMeshAgent>();

        animator.Play(clip.name); // Reproducir la animación de guardia
        agent.SetDestination(guardPoint); // Establecer el destino al punto de guardia

        return base.Run(owner);
    }
}

