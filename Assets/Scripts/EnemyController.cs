using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5f;    // Kecepatan gerakan musuh
    public float chaseRange = 10f;  // Jarak pandang musuh
    public float attackRange = 1f;  // Jarak serangan musuh
    public float attackDamage = 10f;  // Kekuatan serangan musuh

    private Transform target;   // Transform pemain
    private Rigidbody2D rb;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Mendapatkan jarak antara musuh dan pemain
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // Jika pemain dalam jarak pandang, kejar pemain
        if (distanceToTarget <= chaseRange)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        // Mengatur arah gerakan musuh ke arah pemain
        Vector2 direction = (target.position - transform.position).normalized;
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));

        // Mengatur rotasi musuh agar menghadap ke arah pemain
        Vector3 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        // Jika pemain dalam jarak serangan, serang pemain
        if (Vector2.Distance(transform.position, target.position) <= attackRange)
        {
            
        }
    }
}
