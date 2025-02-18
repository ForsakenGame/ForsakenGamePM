using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HealthController : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public HealthBar healthBar;

    private Player_Controller _playerController;
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        _playerController = GetComponent<Player_Controller>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            Heal(1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0)
        {
            _playerController.Die();
        }
    }

    private void Heal(int health)
    {
        currentHealth += health;
        healthBar.SetHealth(currentHealth);
    }

    //internal void TakeDamage(int v)
    //{
    //  throw new NotImplementedException();
    //}
}