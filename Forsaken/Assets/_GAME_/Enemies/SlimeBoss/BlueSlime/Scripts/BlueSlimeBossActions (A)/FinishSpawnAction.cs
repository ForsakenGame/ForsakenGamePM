using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FinishSpawnAction (A)", menuName = "ScriptableObjects/Actions/EndSpawnAction")]
public class FinishSpawnAction : Action
{
    public override bool Check(GameObject owner)
    {
        // Obtener el componente Animator del objeto que está ejecutando esta acción
        Animator animator = owner.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("No se encontró un componente Animator en el objeto: " + owner.name);
            return false; // No hay Animator, no se puede hacer nada
        }

        // Obtener el estado actual de la animación en la capa 0
        AnimatorStateInfo animStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Verificar si la animación ha terminado (normalizedTime >= 1)
        if (animStateInfo.normalizedTime >= 1 && !animator.IsInTransition(0))
        {
            Debug.Log("La animación ha terminado.");
            return true; // La animación ha terminado, se puede cambiar de estado
        }

        return false; // La animación no ha terminado, no se cambia de estado aún
    }

    public override void DrawGizmos(GameObject owner)
    {
        // Este método es opcional y se puede usar para dibujar Gizmos en la escena si es necesario.
    }
}
