using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if(collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(100);
            }  
        }
        gameObject.SetActive(false);
    }
}
