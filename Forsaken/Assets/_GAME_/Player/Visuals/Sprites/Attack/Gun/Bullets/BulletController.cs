using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed, timeAlive;
    public Rigidbody2D bulletRb;
    private Vector2 direction;

    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        bulletRb.velocity = direction * bulletSpeed;
    }

    public void StartDestroyTimer()
    {
        StartCoroutine(DestroyAfterTime(3f)); 
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        DestroyBullet(); 
    }

    public void SetDirection(Vector2 givenDirection)
    {
        direction = givenDirection;
        StartDestroyTimer();
    }

    public void DestroyBullet()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        DestroyBullet();
    }
}
