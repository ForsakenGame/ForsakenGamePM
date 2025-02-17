using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletç : MonoBehaviour
{
    public float bulletSpeed, timeAlive;
    public Rigidbody2D bulletRb;
    private Vector2 direction;
    private float currentTime = 0;
    void Start()
    {
        bulletRb = GetComponent <Rigidbody2D>();
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        bulletRb.velocity = direction * bulletSpeed;
    }

    public void SetDirection(Vector2 givenDirection)
    {
        direction = givenDirection;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // TRAERSE EL COMPONENTE DE LOS ENEMIGOS
        // GreenZombie gz = col.GetComponent<GreenZombie>();

    }
}
