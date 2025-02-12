using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChaseState (S)", menuName = "ScriptableObjects/States/ChaseState")]
public class ChaseState : State
{
    private GameObject player; 
    private NavMeshAgent navComponent; 

    public override void OnStateEnter(GameObject owner)
    {
        player = player.GetComponent<PlayerMovement>().gameObject;

        navComponent = owner.GetComponent<NavMeshAgent>();
    }

    public override State Run(GameObject owner)
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;

            navComponent.SetDestination(playerPosition);
        }
        else
        {
            Debug.LogWarning("Error jugador.");
        }

        return base.Run(owner);
    }
}

