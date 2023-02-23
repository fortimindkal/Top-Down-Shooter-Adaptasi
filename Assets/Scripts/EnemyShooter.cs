﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy
{
    public float shootRange = 5f;   // Jarak serangan tembak
    public float fireRate = 1f;     // Kecepatan tembakan

    public Transform firePoint;
    public GameObject projectilePrefab; // Prefab proyektil yang ditembakkan
    private float _fireTime = 0f;  // Waktu selanjutnya musuh menembak

    protected override void Update()
    {
        base.Update();

        // Jika pemain dalam jarak serangan tembak, tembak pemain
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget <= shootRange && Time.time >= _fireTime)
        {
            _fireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    protected override void ChasePlayer()
    {
        // Mengatur arah gerakan musuh ke arah pemain
        Vector2 direction = (target.position - transform.position).normalized;
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));

        // Mengatur rotasi musuh agar menghadap ke arah pemain
        Vector3 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void Shoot()
    {
        // Membuat proyektil baru dan mengarahkannya ke arah pemain
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Vector2 shootDirection = (target.position - firePoint.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = shootDirection * 10f;
    }
}
