using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed, timeAlive;
    public Rigidbody2D bulletRb;
    private Vector2 direction;
    public float damage = 1f;

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
        GreenZombie_Controller greenZombie = col.GetComponent<GreenZombie_Controller>();
        OrangeZombie_Controller orangeZombie = col.GetComponent<OrangeZombie_Controller>();
        if (greenZombie != null)
        {
            greenZombie.TakeDamage(damage); // Aplica daño al zombie
        }
        else if (orangeZombie != null)
        {
            orangeZombie.TakeDamage(damage);
        }

            DestroyBullet(); // Destruye la bala después del impacto
        DestroyBullet();
    }
}
