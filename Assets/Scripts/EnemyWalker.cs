using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : Enemy
{
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
}
