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

        CircleShoot shooter = owner.GetComponent<CircleShoot>();

        owner.GetComponent<NavMeshAgent>().SetDestination(guardPoint);

        Debug.Log(owner.transform.position + "   ///   " + guardPoint);

        if (Vector3.Distance(owner.transform.position, guardPoint) < 0.1f)
        {
            Debug.Log("Ha entrado, las posiciones son prácticamente iguales");

            //guardPoint = GetGuardPoint(); // Asignar un nuevo punto de patrulla

            Debug.Log("Nuevo punto de guardia: " + guardPoint);
        }

        return base.Run(owner);
    }

}
