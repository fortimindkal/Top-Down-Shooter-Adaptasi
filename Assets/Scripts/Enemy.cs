using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health = 100f;
    public float moveSpeed = 5f;    // Kecepatan gerakan musuh
    public float chaseRange = 10f;  // Jarak pandang musuh
    public float attackRange = 8f;  // Jarak serangan musuh
    public float attackDamage = 5f;  // Kekuatan serangan musuh

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
            ChasePlayer();
        }
    }

    protected abstract void ChasePlayer();

    protected virtual void AttackPlayer()
    {
        // Menyerang pemain dengan kekuatan serangan yang telah ditentukan
        target.GetComponent<PlayerController>().TakeDamage(attackDamage);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Menghilangkan game object enemy dari scene
        Destroy(gameObject);
    }
}