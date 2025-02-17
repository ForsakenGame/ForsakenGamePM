using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BotNavigation : MonoBehaviour
{
    public NavMeshAgent agent; // Referencia al NavMeshAgent del bot
    public NavMeshSurface navMeshSurface; // Referencia al NavMesh en la escena
    public float range = 10f; // Rango de movimiento en X e Y
    private float fixedZ; // Z fija del bot
    public int maxAttempts = 10; // Intentos para encontrar una posición válida

    void Start()
    {
        if (agent == null)
        {
            Debug.LogError(" No se ha asignado un NavMeshAgent al bot.");
            return;
        }

        if (navMeshSurface == null)
        {
            Debug.LogError(" No se ha asignado un NavMeshSurface en el script.");
            return;
        }

        fixedZ = transform.position.z; // Mantiene la Z fija en la posición inicial
        MoveToRandomNavMeshPosition(); // Mueve el bot al inicio
    }

    public void MoveToRandomNavMeshPosition()
    {
        Bounds bounds = navMeshSurface.GetComponent<Collider>().bounds; // Obtiene los límites del NavMesh

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x), // X aleatoria dentro del NavMesh
                Random.Range(bounds.min.y, bounds.max.y), // Y aleatoria dentro del NavMesh
                fixedZ // Z fija
            );

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position); // Envía al bot a la nueva posición
                Debug.Log("Bot moviéndose a: " + hit.position);
                return; // Sale del bucle al encontrar una posición válida
            }
        }

        Debug.LogWarning(" No se encontró una posición válida en el NavMesh.");
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToRandomNavMeshPosition(); // Cuando llega a su destino, genera una nueva posición
        }
    }
}
