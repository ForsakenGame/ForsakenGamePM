using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] GameObject enemyPrefab;   
    [SerializeField] Transform player;          
    [SerializeField] Tilemap groundTilemap;    
    [SerializeField] int maxEnemies;            

    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        GenerateEnemies();  
    }

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
