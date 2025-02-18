using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public List<GameObject> bulletPool;
    public int poolSize = 10;

    private void Start()
    {
        AddBulletToPool(poolSize);
    }

    private void AddBulletToPool(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
            bullet.transform.parent = transform;
        }
    }

    public GameObject RequestBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeSelf)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        // Si no hay balas disponibles, crea una nueva
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(true);
        bulletPool.Add(newBullet);
        return newBullet;
    }
}
