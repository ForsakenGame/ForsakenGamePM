using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolAttackState : State
{
    public float range = 10f; // Rango de búsqueda
    public int maxAttempts = 10; // Número máximo de intentos
    public AudioClip clip;
    public NavMeshSurface navMeshSurface;

    public override State Run(GameObject owner)
    {

        Animator animator = owner.GetComponent<Animator>();

        RandomNavMeshPoint(range, maxAttempts, navMeshSurface, owner);

        animator.Play(clip.name);

        return base.Run(owner);
    }


    private Vector3 RandomNavMeshPoint(float range, int maxattemps, NavMeshSurface navMesh, GameObject owner)
    {

            Vector3 randomPoint = navMesh.transform.position + new Vector3(
                Random.Range(-range, range),
                0, // Mantiene la altura
                Random.Range(-range, range));

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, range, NavMesh.AllAreas))
            {
                return hit.position;
              
            }
           
        

        return Vector3.zero;
    }


}
