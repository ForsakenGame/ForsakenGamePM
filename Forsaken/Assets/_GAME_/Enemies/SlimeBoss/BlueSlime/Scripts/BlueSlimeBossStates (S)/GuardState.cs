using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "GuardState (S)", menuName = "ScriptableObjects/States/GuardState")]
public class GuardState : State
{
    public Vector3 guardPoint;
    public AnimationClip clip;
    private NavMeshSurface navMeshSurface;
    private NavMeshAgent agent;
    private bool hasReachedGuardPoint = false;
    private bool firstGuardPointAssigned = false;

    public override State Run(GameObject owner)
    {
        if (agent == null)
            agent = owner.GetComponent<NavMeshAgent>();

        if (navMeshSurface == null)
        {
            navMeshSurface = GameObject.FindObjectOfType<NavMeshSurface>();
            if (navMeshSurface == null)
            {
                Debug.LogError("No se encontró NavMeshSurface en la escena.");
                return base.Run(owner);
            }
            Debug.Log("NavMeshSurface encontrado: " + navMeshSurface.name);
        }

        Animator animator = owner.GetComponent<Animator>();
        animator.Play(clip.name);

        if (!firstGuardPointAssigned)
        {
            guardPoint = GetRandomPointInNavMesh();
            firstGuardPointAssigned = true;
        }

        if (!hasReachedGuardPoint)
        {
            agent.SetDestination(guardPoint);
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.magnitude == 0)
        {
            if (!hasReachedGuardPoint)
            {
                hasReachedGuardPoint = true;
                Debug.Log("El Boss ha llegado al punto de guardia.");
                OnGuardPointReached();
            }
        }

        return base.Run(owner);
    }

    private void OnGuardPointReached()
    {
        Debug.Log("Generando nuevo punto de guardia...");
        guardPoint = GetRandomPointInNavMesh();
        hasReachedGuardPoint = false;
        agent.SetDestination(guardPoint);
    }

    private Vector3 GetRandomPointInNavMesh()
    {
        if (navMeshSurface == null) return guardPoint;

        Bounds bounds = navMeshSurface.GetComponent<Collider>().bounds;

        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(bounds.min.x + 2, bounds.max.x - 2),
                Random.Range(bounds.min.y, bounds.max.y),  // Ahora la Y también cambia dentro del rango del NavMesh
                Random.Range(bounds.min.z + 2, bounds.max.z - 2)
            );

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 10.0f, NavMesh.AllAreas))
            {
                if (Vector3.Distance(hit.position, guardPoint) > 5f)
                {
                    Debug.Log("Nuevo punto de guardia en NavMesh: " + hit.position);
                    return hit.position;
                }
            }
        }

        Debug.LogWarning("No se encontró una posición válida en el NavMesh. Probando en área más grande.");

        if (NavMesh.SamplePosition(bounds.center, out NavMeshHit fallbackHit, 20.0f, NavMesh.AllAreas))
        {
            return fallbackHit.position;
        }

        return guardPoint;
    }
}
