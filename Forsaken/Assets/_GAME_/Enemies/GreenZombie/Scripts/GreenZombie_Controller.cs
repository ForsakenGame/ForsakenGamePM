using System.Collections;
using UnityEngine;

public class GreenZombie_Controller : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float speed = 2f;
    public LayerMask obstacleLayer;
    public float stopDistance = 1f;
    public float attackDistance = 0.5f;
    public float attackRange = 1.0f; // Rango para comenzar el ataque

    public AnimationClip frontAnimation;
    public AnimationClip backAnimation;
    public AnimationClip leftAnimation;
    public AnimationClip rightAnimation;
    public AnimationClip idleAnimation;

    public AnimationClip runFrontAnimation;
    public AnimationClip runBackAnimation;
    public AnimationClip runLeftAnimation;
    public AnimationClip runRightAnimation;

    public AnimationClip attackFrontAnimation;
    public AnimationClip attackBackAnimation;
    public AnimationClip attackLeftAnimation;
    public AnimationClip attackRightAnimation;

    public AnimationClip deathFrontAnimation;
    public AnimationClip deathBackAnimation;
    public AnimationClip deathLeftAnimation;
    public AnimationClip deathRightAnimation;

    public float maxHealth = 2f; // Vida máxima del zombie
    private float currentHealth; // Vida actual del zombie

    private bool isPlayerInRange = false;
    private Animator anim;
    private Rigidbody2D rb;
    private bool isAttacking = false;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        currentHealth = maxHealth; // Inicializar la vida al máximo
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            isPlayerInRange = true;

            if (distanceToPlayer > stopDistance)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, distanceToPlayer, obstacleLayer);

                if (hit.collider == null)
                {
                    Vector3 direction = (player.position - transform.position).normalized;

                    if (distanceToPlayer > detectionRadius / 2)
                    {
                        rb.MovePosition(transform.position + direction * speed * 1.5f * Time.deltaTime);
                        PlayMovementAnimation(direction, true);
                    }
                    else
                    {
                        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
                        PlayMovementAnimation(direction, false);
                    }
                }
            }
            else if (distanceToPlayer <= attackRange && !isAttacking) // Verifica el rango de ataque
            {
                Attack();
            }
        }
        else
        {
            isPlayerInRange = false;
            anim.Play(idleAnimation.name);
        }
    }

    void PlayMovementAnimation(Vector3 direction, bool isRunning)
    {
        AnimationClip animationToPlay = null;

        if (isRunning)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                animationToPlay = direction.x > 0 ? runRightAnimation : runLeftAnimation;
            }
            else
            {
                animationToPlay = direction.y > 0 ? runFrontAnimation : runBackAnimation;
            }
        }
        else
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                animationToPlay = direction.x > 0 ? rightAnimation : leftAnimation;
            }
            else
            {
                animationToPlay = direction.y > 0 ? frontAnimation : backAnimation;
            }
        }

        if (animationToPlay != null)
        {
            anim.Play(animationToPlay.name);
        }
    }

    void Attack()
    {
        if (isAttacking || isDead) return; // No atacar si ya está atacando o muerto

        isAttacking = true;

        AnimationClip animationToPlay = null;
        Vector3 direction = (player.position - transform.position).normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animationToPlay = direction.x > 0 ? attackRightAnimation : attackLeftAnimation;
        }
        else
        {
            animationToPlay = direction.y > 0 ? attackFrontAnimation : attackBackAnimation;
        }

        if (animationToPlay != null)
        {
            anim.Play(animationToPlay.name);
        }

        // Lógica de ataque (ej. daño al jugador) aquí
        // ...

        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;

        AnimationClip animationToPlay = null;
        Vector3 direction = (player.position - transform.position).normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animationToPlay = direction.x > 0 ? deathRightAnimation : deathLeftAnimation;
        }
        else
        {
            animationToPlay = direction.y > 0 ? deathFrontAnimation : deathBackAnimation;
        }

        if (animationToPlay != null)
        {
            anim.Play(animationToPlay.name);
        }

        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        StartCoroutine(DisappearAfterDeath());
    }

    IEnumerator DisappearAfterDeath()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // Espera a que termine la animación de muerte
        Destroy(gameObject); // Destruye el objeto zombie
    }
}