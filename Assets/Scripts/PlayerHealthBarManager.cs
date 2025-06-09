using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarManager : MonoBehaviour
{
    [SerializeField]
    private Slider m_HealthSlider;

    [SerializeField]
    private float m_MaxHealth = 1f;

    private float m_CurrentHealth;
    
    
    private void Start()
    {
        m_CurrentHealth = m_MaxHealth;
        m_HealthSlider.maxValue = m_MaxHealth;
        m_HealthSlider.value = m_CurrentHealth;
    }

    private void OnTriggerEnter(Collider other)
    {  
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyMissile"))
        {
            TakeDamage(0.1f);
        }
    }

    public void TakeDamage(float amount)
    {
        m_CurrentHealth -= amount;
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0, m_MaxHealth);
        m_HealthSlider.value = m_CurrentHealth;

        HurtEffect.Instance?.PlayHurtEffect();

        if (m_CurrentHealth <= 0)
        {
            Debug.Log("Lose------");
            GeneralEvents.OnGameResult?.Invoke(false);
        }
    }
}