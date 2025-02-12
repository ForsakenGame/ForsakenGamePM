using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackState (S)", menuName = "ScriptableObjects/States/AttackState")]
public class AttackState : State
{
    public float intervalAttack = 0.5f; // Intervalo de 0.5 segundos entre ataques
    public AnimationClip clip;          // Clip de animación de ataque
    private float nextAttackTime;     // Tiempo para el siguiente ataque

  

    public override State Run(GameObject owner)
    {
        nextAttackTime += Time.deltaTime;

        // Obtener el Animator del owner para reproducir la animación de ataque
        Animator animator = owner.GetComponent<Animator>();

        // Ejecutar la animación de ataque
        animator.Play(clip.name);

        // Obtener el script CircleShoot para hacer que dispare
        CircleShoot shooter = owner.GetComponent<CircleShoot>();

        Debug.Log("Time.time: " + Time.time + ", nextAttackTime: " + nextAttackTime);

        // Verificar si ha pasado suficiente tiempo para disparar de nuevo
        if (nextAttackTime >= intervalAttack)
        {
            shooter.Shoot(); // Ejecutar el disparo desde el slime

            // Establecer el tiempo del siguiente disparo
            nextAttackTime = 0;

            Debug.Log("Disparo realizado. El siguiente será en " + intervalAttack + " segundos.");
        }

        // Retornar el estado actual o manejar la transición a otros estados
        return base.Run(owner);
    }
}
