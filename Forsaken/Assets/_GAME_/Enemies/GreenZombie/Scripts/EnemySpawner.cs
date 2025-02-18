using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
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
            GreenZombie_Controller enemyController = newEnemy.GetComponent<GreenZombie_Controller>();
            enemies.Add(newEnemy);
        }
    }

    // Finds a valid random position within the ground tilemap and returns its world coordinates
    Vector3 GetRandomPositionInGround()
    {
        List<Vector3> validPositions = new List<Vector3>();
        BoundsInt bounds = groundTilemap.cellBounds;

        // Recorrer todas las celdas dentro de los límites del Tilemap
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                if (groundTilemap.HasTile(cellPosition))
                {
                    validPositions.Add(groundTilemap.CellToWorld(cellPosition) + new Vector3(0.5f, 0.5f, 0f)); // Ajustar para centrar en el tile
                }
            }
        }

        // Si no hay posiciones válidas, retornar la posición actual del spawner
        if (validPositions.Count == 0)
        {
            Debug.LogError("No hay tiles válidos para spawnear enemigos.");
            return transform.position;
        }

        // Elegir una posición aleatoria de la lista
        return validPositions[Random.Range(0, validPositions.Count)];
    }

}
