using System.Collections;
using UnityEngine;

public class GreenZombie_Controller : MonoBehaviour
{
    // Reference to the player
    public Transform player;

    // Detection and movement settings
    public float detectionRadius = 10f;  // Radius within which the zombie detects the player
    public float speed = 2f;             // Zombie's movement speed
    public LayerMask obstacleLayer;      // Layers that may block the zombie's view
    public float stopDistance = 1f;      // Distance at which zombie stops before reaching the player
    public float attackRange = 1.0f;     // Range within which the zombie starts attacking

    // Movement animations
    public AnimationClip frontAnimation, backAnimation, leftAnimation, rightAnimation, idleAnimation;
    public AnimationClip runFrontAnimation, runBackAnimation, runLeftAnimation, runRightAnimation;

    // Attack animations
    public AnimationClip attackFrontAnimation, attackBackAnimation, attackLeftAnimation, attackRightAnimation;

    // Death animations
    public AnimationClip deathFrontAnimation, deathBackAnimation, deathLeftAnimation, deathRightAnimation;

    // Health system
    public float maxHealth = 2f;  // Maximum health of the zombie
    private float currentHealth;   // Current health

    // State variables
    private bool isPlayerInRange = false;
    private bool isAttacking = false;
    private bool isDead = false;

    // Components
    private Animator anim;
    private Rigidbody2D rb;

    // Initialize variables and components
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Find the player by tag
        anim = GetComponent<Animator>();  // Get the Animator component
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        rb.bodyType = RigidbodyType2D.Kinematic; // Set Rigidbody to kinematic for manual movement

        currentHealth = maxHealth;  // Initialize health to maximum
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;  // Skip update if the zombie is dead

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);  // Distance to the player

        // Check if the player is within detection range
        if (distanceToPlayer <= detectionRadius)
        {
            isPlayerInRange = true;

            // If the player is within stop distance, stop moving, else move towards player
            if (distanceToPlayer > stopDistance)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, distanceToPlayer, obstacleLayer);

                if (hit.collider == null)  // If no obstacles in the way
                {
                    Vector3 direction = (player.position - transform.position).normalized;  // Direction towards player

                    // Move faster if the player is far, otherwise move at normal speed
                    if (distanceToPlayer > detectionRadius / 2)
                    {
                        rb.MovePosition(transform.position + direction * speed * 1.5f * Time.deltaTime);
                        PlayMovementAnimation(direction, true);  // Play running animation
                    }
                    else
                    {
                        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
                        PlayMovementAnimation(direction, false);  // Play walking animation
                    }
                }
            }
            // If the player is close enough to attack, initiate attack
            else if (distanceToPlayer <= attackRange && !isAttacking)
            {
                Attack();
            }
        }
        else
        {
            isPlayerInRange = false;
            anim.Play(idleAnimation.name);  // Play idle animation if player is not in range
        }
    }

    // Handles zombie movement and animation based on direction and speed
    void PlayMovementAnimation(Vector3 direction, bool isRunning)
    {
        AnimationClip animationToPlay = null;

        if (isRunning)  // If the zombie is running
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
        else  // If the zombie is walking
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
            anim.Play(animationToPlay.name);  // Play the appropriate movement animation
        }
    }

    // Handles attacking the player
    void Attack()
    {
        if (isAttacking || isDead) return;  // Prevent attacking if already attacking or dead

        isAttacking = true;

        AnimationClip animationToPlay = null;
        Vector3 direction = (player.position - transform.position).normalized;

        // Choose the correct attack animation based on player direction
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
            anim.Play(animationToPlay.name);  // Play the attack animation
        }

        // Apply damage to the player if in range
        if (player != null)
        {
            Player_HealthController playerHealth = player.GetComponent<Player_HealthController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);  // Deal 1 damage to the player
            }
            else
            {
                Debug.LogError("Error: Player health component missing.");
            }
        }
        else
        {
            Debug.LogError("Error: Player not found.");
        }
        StartCoroutine(ResetAttack());  // Wait for the attack animation to finish before allowing another attack
    }

    // Resets the attack state after the attack animation finishes
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;  // Reset attacking flag
    }

    // Reduces the zombie's health when taking damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;  // Subtract damage from current health

        // If health drops to zero, the zombie dies
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    // Handles the zombie's death and plays the death animation
    public void Die()
    {
        isDead = true;

        AnimationClip animationToPlay = null;
        Vector3 direction = (player.position - transform.position).normalized;

        // Choose the correct death animation based on player direction
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
            anim.Play(animationToPlay.name);  // Play the death animation
        }

        rb.velocity = Vector2.zero;  // Stop all movement
        GetComponent<Collider2D>().enabled = false;  // Disable collider to prevent interaction
        this.enabled = false;  // Disable the zombie controller script

        StartCoroutine(DisappearAfterDeath());  // Start a coroutine to destroy the zombie after the death animation
    }

    // Destroys the zombie object after the death animation finishes
    IEnumerator DisappearAfterDeath()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);  // Wait for the death animation to finish
        Destroy(gameObject);  // Destroy the zombie object
    }
}
