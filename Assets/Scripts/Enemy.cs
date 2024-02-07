using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float enemyHealth = 20.0f;

    public void TakeDamage(float damageAmount)
    {
        enemyHealth -= damageAmount;
        if(enemyHealth <= 0.0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
