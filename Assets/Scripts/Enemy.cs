using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Controller
{
    public float moveSpeed = 5f;    // Kecepatan gerakan musuh
    public float chaseRange = 10f;  // Jarak pandang musuh
    public float attackRange = 8f;  // Jarak serangan musuh
    public float attackDamage = 5f;  // Kekuatan serangan musuh
    public float attackDelay = 0f; // Waktu delay menyerang

    protected Transform target;   // Transform pemain
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        // Mendapatkan jarak antara musuh dan pemain
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // Jika pemain dalam jarak pandang, kejar pemain
        if (distanceToTarget <= chaseRange)
        {
            FindTarget();
        }
    }

    protected virtual void AttackPlayer()
    {
        // Menyerang pemain dengan kekuatan serangan yang telah ditentukan
        target.GetComponent<PlayerController>().TakeDamage(attackDamage);
    }

    protected override void Move()
    {
        // Mengatur arah gerakan musuh ke arah pemain
        Vector2 direction = (target.position - transform.position).normalized;
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    protected override void Rotate()
    {
        // Mengatur rotasi musuh agar menghadap ke arah pemain
        Vector3 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    protected virtual void FindTarget()
    {
        Move();
        Rotate();
    }

}