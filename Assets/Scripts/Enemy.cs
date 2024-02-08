using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float enemyHealth = 20.0f;
    [SerializeField] GameObject enemyDeathEffect;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip barealExplosion;

   
    public void TakeDamage(float damageAmount)
    {
        enemyHealth -= damageAmount;
        if(enemyHealth <= 0.1f)
        {
            Die();
        }
    }

    void Die()
    {
        GameObject deathEffect = Instantiate(enemyDeathEffect, transform.position, Quaternion.identity);
        source.PlayOneShot(barealExplosion);

        Destroy(deathEffect, 2.5f);
        Destroy(gameObject);
    }
}
