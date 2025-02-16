using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenZombie_Controller : MonoBehaviour
{
    public Transform player;                  // El jugador (a asignar automáticamente con el Tag)
    public float detectionRadius = 10f;       // Radio de detección
    public float speed = 2f;                  // Velocidad de movimiento
    public LayerMask obstacleLayer;           // Capa para detectar obstáculos
    public float stopDistance = 1f;           // Distancia mínima para no mover al zombie cuando está cerca del jugador
    public float attackDistance = 0.5f;       // Distancia para comenzar la animación de ataque

    // Clips de animación asignables desde el Inspector
    public AnimationClip backAnimation;      // Animación para moverse hacia abajo
    public AnimationClip frontAnimation;     // Animación para moverse hacia arriba
    public AnimationClip leftAnimation;      // Animación para moverse hacia la izquierda
    public AnimationClip rightAnimation;     // Animación para moverse hacia la derecha
    public AnimationClip idleAnimation;      // Animación cuando está quieto
    public AnimationClip attackAnimation;    // Animación de ataque

    private bool isPlayerInRange = false;     // Para verificar si el jugador está dentro del rango
    private Animation zombieAnimation;        // Componente de animación del zombie
    private Rigidbody2D rb;                   // Rigidbody2D del zombie

    void Start()
    {
        // Asignar automáticamente al jugador usando su Tag
        if (player == null) // Verificamos si ya está asignado
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;  // Asigna el transform del jugador automáticamente
        }

        // Asegurarnos de que el componente de Animation y Rigidbody2D estén asignados
        zombieAnimation = GetComponent<Animation>();
        rb = GetComponent<Rigidbody2D>();

        // Asegurarnos de que el Rigidbody2D está en modo cinemático
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        // Obtener la distancia entre el enemigo y el jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // El jugador está dentro del rango de detección
            isPlayerInRange = true;

            // Si la distancia al jugador es mayor que la distancia de parada, el zombie se mueve
            if (distanceToPlayer > stopDistance)
            {
                // Verificar si hay un obstáculo entre el enemigo y el jugador
                RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, distanceToPlayer, obstacleLayer);

                if (hit.collider == null)  // Si no hay obstáculos entre el enemigo y el jugador
                {
                    // Mover al enemigo hacia el jugador (utilizando Rigidbody2D)
                    Vector3 direction = (player.position - transform.position).normalized;
                    rb.MovePosition(transform.position + direction * speed * Time.deltaTime);

                    // Determinar en qué dirección se mueve el zombie y seleccionar la animación correspondiente
                    PlayMovementAnimation(direction);
                }
            }
            // Si la distancia es menor o igual a attackDistance, detenerse y reproducir animación de ataque
            else if (distanceToPlayer <= attackDistance)
            {
                // El zombie se detiene y reproduce la animación de ataque
                PlayAnimation(attackAnimation);
            }
        }
        else
        {
            // El jugador está fuera del rango de detección
            isPlayerInRange = false;

            // Reproducir animación de "inactividad" o "quieto"
            PlayAnimation(idleAnimation);
        }
    }

    // Función para reproducir una animación
    void PlayAnimation(AnimationClip animationClip)
    {
        if (zombieAnimation != null && !zombieAnimation.isPlaying)
        {
            zombieAnimation.Play(animationClip.name);
        }
    }

    // Determina la animación de movimiento según la dirección hacia el jugador
    void PlayMovementAnimation(Vector3 direction)
    {
        if (zombieAnimation == null) return;

        // Solo reproducir las animaciones de movimiento si no se está atacando
        if (zombieAnimation.isPlaying && zombieAnimation.clip == attackAnimation)
            return;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Movimiento en el eje X (izquierda o derecha)
            if (direction.x > 0)
            {
                // El jugador está a la derecha
                PlayAnimation(rightAnimation);
            }
            else
            {
                // El jugador está a la izquierda
                PlayAnimation(leftAnimation);
            }
        }
        else
        {
            // Movimiento en el eje Y (arriba o abajo)
            if (direction.y > 0)
            {
                // El jugador está arriba
                PlayAnimation(frontAnimation);
            }
            else
            {
                // El jugador está abajo
                PlayAnimation(backAnimation);
            }
        }
    }
}
