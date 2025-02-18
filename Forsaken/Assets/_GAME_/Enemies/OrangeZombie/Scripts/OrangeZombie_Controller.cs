using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeZombie_Controller : MonoBehaviour
{
    // Reference to the player
    public Transform player;

    // Detection and movement settings
    public float detectionRadius = 10f;  // Distance at which the zombie detects the player
    public float speed = 2f;             // Movement speed of the zombie
    public float stopDistance = 1f;      // Distance at which the zombie stops before reaching the player
    public float attackRange = 1.0f;     // Distance at which the zombie starts attacking

    // Animation clips for different states
    public AnimationClip frontAnimation, backAnimation, leftAnimation, rightAnimation, idleAnimation;
    public AnimationClip runFrontAnimation, runBackAnimation, runLeftAnimation, runRightAnimation;
    public AnimationClip attackFrontAnimation, attackBackAnimation, attackLeftAnimation, attackRightAnimation;
    public AnimationClip deathFrontAnimation, deathBackAnimation, deathLeftAnimation, deathRightAnimation;

    // Health system
    public float maxHealth = 5f;  // Maximum health of the zombie
    private float currentHealth;   // Current health of the zombie

    // State variables
    private bool isPlayerInRange = false; // Tracks if the player is within detection range
    private bool isAttacking = false;     // Checks if the zombie is currently attacking
    private bool isDead = false;          // Checks if the zombie is dead

    // Components
    private Animator anim;
    private Rigidbody2D rb;

    // Initializes the zombie's properties and finds the player
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        currentHealth = maxHealth;
    }

    // Handles zombie behavior each frame, including movement, detection, and attacking
    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            isPlayerInRange = true;

            if (distanceToPlayer > stopDistance)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
                PlayMovementAnimation(direction, distanceToPlayer > detectionRadius / 2);
            }
            else if (distanceToPlayer <= attackRange && !isAttacking)
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

    // Plays the correct movement animation based on the zombie's direction
    void PlayMovementAnimation(Vector3 direction, bool isRunning)
    {
        AnimationClip animationToPlay = null;

        if (isRunning)
            animationToPlay = Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
                ? (direction.x > 0 ? runRightAnimation : runLeftAnimation)
                : (direction.y > 0 ? runFrontAnimation : runBackAnimation);
        else
            animationToPlay = Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
                ? (direction.x > 0 ? rightAnimation : leftAnimation)
                : (direction.y > 0 ? frontAnimation : backAnimation);

        if (animationToPlay != null)
            anim.Play(animationToPlay.name);
    }

    // Handles the zombie's attack behavior and deals damage to the player
    void Attack()
    {
        if (isAttacking || isDead) return;

        isAttacking = true;

        AnimationClip animationToPlay = Mathf.Abs(player.position.x - transform.position.x) > Mathf.Abs(player.position.y - transform.position.y)
            ? (player.position.x > transform.position.x ? attackRightAnimation : attackLeftAnimation)
            : (player.position.y > transform.position.y ? attackFrontAnimation : attackBackAnimation);

        if (animationToPlay != null)
            anim.Play(animationToPlay.name);

        Player_HealthController playerHealth = player.GetComponent<Player_HealthController>();
        if (playerHealth != null)
            playerHealth.TakeDamage(2);

        StartCoroutine(ResetAttack());
    }

    // Waits until the attack animation is complete before allowing another attack
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
    }

    // Reduces the zombie's health when it takes damage
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    // Handles the zombie's death by playing the death animation and disabling its components
    public void Die()
    {
        isDead = true;

        AnimationClip animationToPlay = Mathf.Abs(player.position.x - transform.position.x) > Mathf.Abs(player.position.y - transform.position.y)
            ? (player.position.x > transform.position.x ? deathRightAnimation : deathLeftAnimation)
            : (player.position.y > transform.position.y ? deathFrontAnimation : deathBackAnimation);

        if (animationToPlay != null)
            anim.Play(animationToPlay.name);

        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        StartCoroutine(DisappearAfterDeath());
    }

    // Waits for the death animation to finish before destroying the zombie object
    IEnumerator DisappearAfterDeath()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
