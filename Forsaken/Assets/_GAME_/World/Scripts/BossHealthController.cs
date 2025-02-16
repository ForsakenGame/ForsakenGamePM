using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthController : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public HealthBar healthBar;

    private BossRoomBehaviour _bossScript;
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        _bossScript = GetComponent<BossRoomBehaviour>();    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Heal(1);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            GameManager.instance.isBossAlive = false;
            GameManager.instance.BossEnd();
        }
    }

    private void Heal(int health)
    {
        currentHealth += health;
        healthBar.SetHealth(currentHealth);
    }
}