using UnityEngine;
using System.Collections.Generic;

public class RandomPositionGenerator : MonoBehaviour
{
    [Header("Límites de la Sala")]
    public Vector2 topLeft;       // Esquina superior izquierda (se ingresa en el Inspector)
    public Vector2 bottomRight;   // Esquina inferior derecha (se ingresa en el Inspector)
    public float fixedZ = 0f;     // Z fija (puede modificarse en el Inspector)

    public int maxPositions = 5;  // Número de posiciones a generar
    public float minDistance = 5f; // Distancia mínima entre puntos
    public List<Vector3> positions = new List<Vector3>(); // Lista de posiciones generadas

    // Método para generar una posición aleatoria dentro de los límites ingresados
    public Vector3 GetRandomPosition()
    {
        for (int i = 0; i < 10; i++) // Intentos para evitar bloqueos si no encuentra un punto válido
        {
            float randomX = Random.Range(topLeft.x, bottomRight.x);
            float randomY = Random.Range(bottomRight.y, topLeft.y);
            Vector3 newPoint = new Vector3(randomX, randomY, fixedZ);

            // Si la lista está vacía, aceptar directamente el primer punto
            if (positions.Count == 0)
                return newPoint;

            // Verificar que el nuevo punto tenga al menos minDistance de separación con todos los anteriores
            bool isFarEnough = true;
            foreach (Vector3 pos in positions)
            {
                if (Vector3.Distance(newPoint, pos) < minDistance)
                {
                    isFarEnough = false;
                    break;
                }
            }

            // Si la posición cumple con la distancia mínima, retornarla
            if (isFarEnough)
                return newPoint;
        }

        // Si no encuentra una posición válida después de varios intentos, devolver la última posición
        return positions[positions.Count - 1];
    }

    void Start()
    {
        // Generar posiciones aleatorias dentro de la sala respetando la distancia mínima
        for (int i = 0; i < maxPositions; i++)
        {
            Vector3 newPos = GetRandomPosition();
            positions.Add(newPos);
            Debug.Log("Posición aleatoria generada: " + newPos);
        }
    }
}
