using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float enemyHealth = 20.0f;
    [SerializeField] GameObject enemyDeathEffect;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip barealExplosion;

    private void Start()
    {
    }
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
        GameObject deathEffect = Instantiate(enemyDeathEffect, transform.position, Quaternion.identity);
        source.PlayOneShot(barealExplosion);

        Destroy(deathEffect, 2.0f);
        Destroy(gameObject);
    }
}
