using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] GameObject enemyPrefab;   // Prefabricado del enemigo
    [SerializeField] Transform player;          // Referencia al jugador
    [SerializeField] Tilemap groundTilemap;     // Referencia al Tilemap que representa el suelo
    [SerializeField] int maxEnemies;            // Número máximo de enemigos

    private List<GameObject> enemies = new List<GameObject>(); // Lista para almacenar los enemigos generados

    void Start()
    {
        GenerateEnemies();  // Genera los enemigos cuando inicie el juego
    }

    void GenerateEnemies()
    {
        // Crear enemigos hasta alcanzar el número máximo de enemigos
        for (int i = enemies.Count; i < maxEnemies; i++)
        {
            // Obtener una posición aleatoria dentro del área del "Ground"
            Vector3 randomPosition = GetRandomPositionInGround();

            // Instanciar un nuevo enemigo en la posición aleatoria
            GameObject newEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            // Obtener el controlador del enemigo (GreenZombie_Controller)
            GreenZombie_Controller enemyController = newEnemy.GetComponent<GreenZombie_Controller>();
            if (enemyController != null)
            {
                // Asignar al enemigo el jugador y el punto de guardia
                enemyController.SetPlayer(player);
                enemyController.SetGuardPoint(randomPosition); // Establecer la posición de guardia
            }

            // Agregar el nuevo enemigo a la lista de enemigos
            enemies.Add(newEnemy);
        }
    }

    // Generar una posición aleatoria dentro de las dimensiones del "Ground" (Tilemap)
    Vector3 GetRandomPositionInGround()
    {
        // Obtener las dimensiones del Tilemap
        BoundsInt bounds = groundTilemap.cellBounds;

        Vector3 randomWorldPosition = Vector3.zero;
        bool validPosition = false;

        // Generar posiciones aleatorias dentro del rango del Tilemap
        while (!validPosition)
        {
            int randomX = Random.Range(bounds.xMin, bounds.xMax);
            int randomY = Random.Range(bounds.yMin, bounds.yMax);

            // Verificar si hay un tile en esta celda
            if (groundTilemap.HasTile(new Vector3Int(randomX, randomY, 0)))
            {
                // Convertir las coordenadas del Tilemap a coordenadas del mundo
                randomWorldPosition = groundTilemap.CellToWorld(new Vector3Int(randomX, randomY, 0));

                // Ajustar la posición Y si es necesario
                randomWorldPosition.z = 0f; // Asumimos que es un juego 2D o en el plano X-Z.

                validPosition = true; // Hemos encontrado una posición válida
            }
        }

        return randomWorldPosition;
    }
}
