using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : Enemy
{
    public float attackDelay = 0f; // Waktu delay menyerang
    protected override void Update()
    {
        base.Update();

        // Jika pemain dekat dengan enemy, serang pemain
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget <= attackRange && attackDelay <= 0f)
        {
            AttackPlayer();
            attackDelay = 3f;
            Debug.Log("Attack!");
        }

        if (attackDelay > 0)
        {
            attackDelay -= Time.deltaTime;
        }
        else
        {
            attackDelay = 0;
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
}
