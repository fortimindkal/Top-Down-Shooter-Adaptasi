using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [SerializeField]
    protected float Health = 100;

    protected abstract void Move();
    protected abstract void Rotate();
    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // Menghilangkan game object enemy dari scene
        Destroy(gameObject);
    }
}
