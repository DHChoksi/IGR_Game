using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarManager : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 1f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void OnTriggerEnter(Collider other)
    {  
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyMissile"))
        {
            TakeDamage(0.2f);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;

        HurtEffect.Instance?.PlayHurtEffect();

        if (currentHealth <= 0)
        {
            WinLoseManager.Instance?.ShowResult(false);
        }
    }
}