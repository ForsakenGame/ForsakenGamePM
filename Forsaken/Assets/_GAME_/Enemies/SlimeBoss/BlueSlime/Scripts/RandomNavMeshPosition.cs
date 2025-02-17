using UnityEngine;
using UnityEngine.AI;

public class RandomNavMeshPosition : MonoBehaviour
{
    public float range = 10f; // Rango de búsqueda

    public Vector3 GetRandomNavMeshPosition()
    {
        for (int i = 0; i < 10; i++) // Intentar hasta 10 veces
        {
            Vector3 randomPoint = transform.position + new Vector3(
                Random.Range(-range, range),(float)(-0.43), // Mantiene la altura
                Random.Range(-range, range)
            );

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                return hit.position; // Devolver la posición válida en el NavMesh
            }
        }

        return transform.position; // Si no encuentra una posición válida, devuelve la actual
    }
}
