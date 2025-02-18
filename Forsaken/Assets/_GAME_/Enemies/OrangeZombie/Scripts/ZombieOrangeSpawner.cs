using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZombieOrangeSpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] GameObject enemyPrefab;   // Prefab of the enemy to spawn
    [SerializeField] Transform player;        // Reference to the player
    [SerializeField] Tilemap groundTilemap;   // Tilemap defining the ground where enemies can spawn
    [SerializeField] int maxEnemies;          // Maximum number of enemies allowed

    private List<GameObject> enemies = new List<GameObject>(); // List to track spawned enemies

    // Called at the start of the game, initializes enemy spawning
    void Start()
    {
        GenerateEnemies();
    }

    // Spawns enemies up to the maxEnemies limit, placing them at random valid positions on the ground tilemap
    void GenerateEnemies()
    {
        for (int i = enemies.Count; i < maxEnemies; i++)
        {
            Vector3 randomPosition = GetRandomPositionInGround();
            GameObject newEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            OrangeZombie_Controller enemyController = newEnemy.GetComponent<OrangeZombie_Controller>();
            enemies.Add(newEnemy);
        }
    }

    // Finds a valid random position within the ground tilemap and returns its world coordinates
    Vector3 GetRandomPositionInGround()
    {
        BoundsInt bounds = groundTilemap.cellBounds;
        Vector3 randomWorldPosition = Vector3.zero;
        bool validPosition = false;

        while (!validPosition)
        {
            int randomX = Random.Range(bounds.xMin, bounds.xMax);
            int randomY = Random.Range(bounds.yMin, bounds.yMax);

            if (groundTilemap.HasTile(new Vector3Int(randomX, randomY, 0)))
            {
                randomWorldPosition = groundTilemap.CellToWorld(new Vector3Int(randomX, randomY, 0));
                randomWorldPosition.z = 0f;
                validPosition = true;
            }
        }

        return randomWorldPosition;
    }
}
