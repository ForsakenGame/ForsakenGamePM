using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiSpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform player;
    [SerializeField] float spawnRadius = 10f;
    [SerializeField] int maxEnemies = 6;

    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        GenerateEnemies();
    }

    void GenerateEnemies()
    {
        int enemyCount = Mathf.Min(maxEnemies, enemies.Count);

        for (int i = enemyCount; i < maxEnemies; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-spawnRadius, spawnRadius),
                                                  Random.Range(-spawnRadius, spawnRadius),
                                                  0f);

            GameObject newEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            GreenZombie_Controller enemyController = newEnemy.GetComponent<GreenZombie_Controller>();
            if (enemyController != null)
            {
                enemyController.SetPlayer(player);
            }

            enemies.Add(newEnemy);
        }
    }

}
