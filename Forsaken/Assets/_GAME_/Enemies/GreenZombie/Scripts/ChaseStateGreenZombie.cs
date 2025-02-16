using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChaseStateSZ (S)", menuName = "ScriptableObjects/States/ChaseStateSZ")]
public class ChaseStateGreenZombie : State
{
    private GameObject player; // Variable para guardar la referencia al jugador

    public AnimationClip rightAnim;
    public AnimationClip leftAnim;
    public AnimationClip frontAnim;
    public AnimationClip backAnim;


    public override State Run(GameObject owner)
    {
        // Si el jugador no se ha encontrado (en caso de que no esté activo en la escena)
        if (player == null)
        {
            Debug.LogWarning("Jugador no encontrado.");
            return base.Run(owner); // Si no se encuentra, salir del estado
        }

        // Obtener la posición del jugador
        Vector3 playerPosition = player.transform.position;

        // Mover el NavMeshAgent hacia la posición del jugador
        NavMeshAgent navComponent = owner.GetComponent<NavMeshAgent>();
        navComponent.SetDestination(playerPosition);

        // Obtener el animator del zombie para reproducir las animaciones
        Animator animator = owner.GetComponent<Animator>();

        // Determinar la dirección del movimiento del zombie en relación al jugador
        Vector3 directionToPlayer = playerPosition - owner.transform.position;
        directionToPlayer.y = 0;  // Para ignorar la diferencia de altura

        // Comprobar la dirección de movimiento
        if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.z)) // Movimiento horizontal (izquierda/derecha)
        {
            if (directionToPlayer.x > 0)  // Hacia la derecha
            {
                animator.Play(rightAnim.name);
            }
            else  // Hacia la izquierda
            {
                animator.Play(leftAnim.name);
            }
        }
        else // Movimiento vertical (frente/detrás)
        {
            if (directionToPlayer.z > 0)  // Hacia adelante (frente)
            {
                animator.Play(frontAnim.name);
            }
            else  // Hacia atrás
            {
                animator.Play(backAnim.name);
            }
        }

        return base.Run(owner);
    }
}
