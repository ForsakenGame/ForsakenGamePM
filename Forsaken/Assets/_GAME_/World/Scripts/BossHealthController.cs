using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthController : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public HealthBar healthBar;
    public GameObject bossHealthBar;
    public CinemachineVirtualCamera virtualCamera;

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
            bossHealthBar.SetActive(false);
            GameManager.instance.isBossAlive = false;
            if (virtualCamera != null)
            {
                StartCoroutine(SmoothCameraTransition(5, 1));
            }
            GameManager.instance.BossEnd();
        }
    }

    private void Heal(int health)
    {
        currentHealth += health;
        healthBar.SetHealth(currentHealth);
    }

    private IEnumerator SmoothCameraTransition(float targetSize, float duration)
    {
        float startSize = virtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetSize; // Ensure it reaches the exact target size
    }
}