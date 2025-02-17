using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed, timeAlive;

    private Vector2 _dir = Vector2.zero;
    private Rigidbody2D _rb;
    private float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= timeAlive)
        {
            currentTime = 0;
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = _dir * speed;
    }

    public void SetDirection(Vector2 value)
    {
        _dir = value;
    }
}
