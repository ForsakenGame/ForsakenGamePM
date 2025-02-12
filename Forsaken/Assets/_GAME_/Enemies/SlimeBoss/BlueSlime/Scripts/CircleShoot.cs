using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObjectPool bulletPool;
    public int numberOfBullets = 12;
   
    // Mueve la lógica de disparo a un método público
    public void Shoot()
    {
        // Obtener la posición actual del slime (el objeto donde está este script)
        Vector3 slimePosition = transform.position;

        // Dividir el círculo en 'numberOfBullets' segmentos
        float step = 360f / numberOfBullets;

        // Bucle para generar los disparos en círculo
        for (int i = 0; i < numberOfBullets; i++)
        {
            // Calcular el ángulo para cada bala
            float angle = step * i;

            // Calcular la dirección de disparo usando senos y cosenos
            Vector2 direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad),
                                            Mathf.Cos(angle * Mathf.Deg2Rad));

            // Obtener un objeto de la pool de balas
            GameObject o = bulletPool.GetAvailableObject();

            // Colocar la bala en la posición del slime
            o.transform.position = slimePosition;

            // Activar la bala
            o.SetActive(true);

            // Configurar la dirección de la bala
            o.GetComponent<Bullet>().SetDirection(direction);
        }
    }
}
