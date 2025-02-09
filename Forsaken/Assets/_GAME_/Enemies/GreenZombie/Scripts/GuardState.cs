using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "GuardState (S)", menuName = "ScriptableObjects/States/GuardState")]

public class GuardState : State
{
    public override State Run(GameObject owner)
    {
        GreenZombie_Controller enemyController = owner.GetComponent<GreenZombie_Controller>();
        if (enemyController != null)
        {
            Vector3 guardPoint = enemyController.GetGuardPoint(); 

            NavMeshAgent navMeshAgent = owner.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.SetDestination(guardPoint);  
            }
        }

        return base.Run(owner);
    }
}


