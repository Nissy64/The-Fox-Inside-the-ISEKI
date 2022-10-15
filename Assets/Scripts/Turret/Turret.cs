using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public LayerMask playerLayer;
    public Transform turretMuzzle;
    public GameObject bulletPrefab;
    public float fireRate = 100;
    private float nextTimeToFire = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if(Physics2D.Raycast(turretMuzzle.position, Vector2.right, 48, playerLayer) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1 / fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, turretMuzzle.position, transform.rotation);
    }


}
