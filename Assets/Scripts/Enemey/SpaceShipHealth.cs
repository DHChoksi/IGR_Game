
using UnityEngine;
public class SpaceShipHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public ParticleSystem explosionEffect;
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void ApplyDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0f) Die();
    }
    void Die()
    {
        if (explosionEffect)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
