using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 20;
    public Rigidbody2D rb;

    void Start()
    {
        rb.velocity = transform.right * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Destroy(gameObject);

        if(collider.CompareTag("Teleporter"))
        {
            rb.velocity = -transform.right * bulletSpeed;
        }
    }
}
