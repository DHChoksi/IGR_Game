using UnityEngine;
using UnityEngine.UI;

using static Constants.Constants;

public class Enemy : MonoBehaviour
{
    [SerializeField] 
    private DrownState m_CurrentState = DrownState.Move;

    [Header("Combat")]
    [SerializeField] 
    private float m_PlayerDetectRadius = 10f;
    
    [SerializeField]
    private float m_AttackRange = 6f;
    
    [SerializeField]
    private float m_AttackCooldown = 2f;
   
    [SerializeField] 
    private float m_MaxHealth = 2;
    
    [SerializeField]
    private GameObject m_MissilePrefab;
   
    [SerializeField] 
    private Transform m_MissileSpawnPoint;
   
    [SerializeField] 
    private GameObject m_BlastEffectPrefab;

    [SerializeField]
    private float m_MissileSpeed = 550f;

    [Header("UI")]
    public Slider m_HealthSlider;


    private float m_Health;
    private float m_AttackTimer;

    private Transform m_PlayerHead;
    private Transform m_ClusterCenter;
    private Vector3 m_LocalOffset;

    private Transform[] m_RocketTips;

    public void SetPlayerHead(Transform head)
    {
        m_PlayerHead = head;

        // Optional: Automatically find rocket tips
        m_RocketTips = new Transform[3];
        m_RocketTips[0] = GameObject.Find("RocketTip1")?.transform;
        m_RocketTips[1] = GameObject.Find("RocketTip2")?.transform;
        m_RocketTips[2] = GameObject.Find("RocketTip3")?.transform;
    }

    public void SetClusterParent(Transform parent, Vector3 offset)
    {
        m_ClusterCenter = parent;
        m_LocalOffset = offset;
    }

    private void Start()
    {
        m_Health = m_MaxHealth;
        if (m_HealthSlider != null)
        {
            m_HealthSlider.maxValue = m_MaxHealth;
            m_HealthSlider.value = m_Health;
        }
    }
    private void Update()
    {
        if (m_CurrentState == DrownState.Death || m_PlayerHead == null)
            return;

        float distToPlayer = Vector3.Distance(transform.position, m_PlayerHead.position);

        switch (m_CurrentState)
        {
            case DrownState.Move:
                if (m_ClusterCenter != null)
                {
                    LookAt(m_ClusterCenter.position + m_ClusterCenter.forward * 10f); // Look ahead
                }
                 
                if (distToPlayer <= m_PlayerDetectRadius)
                    m_CurrentState = DrownState.PlayerDetected;
                break;


            case DrownState.PlayerDetected:
                LookAt(m_PlayerHead.position);

                if (distToPlayer <= m_AttackRange)
                {
                    m_CurrentState = DrownState.Attack;
                    m_AttackTimer = m_AttackCooldown;
                }
                break;

            case DrownState.Attack:
                LookAt(m_PlayerHead.position);
                m_AttackTimer -= Time.deltaTime;

                if (m_AttackTimer <= 0f)
                {
                    FireMissile();
                    m_AttackTimer = m_AttackCooldown;
                }

                if (distToPlayer > m_AttackRange)
                    m_CurrentState = DrownState.PlayerDetected;
                break;
        }
    }

    private void MaintainClusterOffset()
    {
        if (m_ClusterCenter == null) return;

        Vector3 targetPosition = m_ClusterCenter.TransformPoint(m_LocalOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2f);
        LookAt(m_ClusterCenter.position + m_ClusterCenter.forward * 10f);
    }

    private void LookAt(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        if (dir.magnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }
    }

    private void FireMissile()
    {
        if (m_MissilePrefab == null || m_MissileSpawnPoint == null || m_PlayerHead == null) 
            return;

        GameObject missile = Instantiate(m_MissilePrefab, m_MissileSpawnPoint.position, Quaternion.identity);

        Vector3 direction = (m_PlayerHead.position - m_MissileSpawnPoint.position).normalized;
        missile.transform.rotation = Quaternion.LookRotation(direction);
         
        Rigidbody rb = missile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Adjust as needed
            rb.linearVelocity = direction * m_MissileSpeed;
            rb.useGravity = false;   // Optional: depends on your missile type
        }

        Destroy(missile, 5f);
    }

    public void SetClusterParent(Transform parent)
    {
        m_ClusterCenter = parent;
    }

    public void TakeDamage()
    {
        m_Health = m_Health - 0.5f;
        if (m_HealthSlider != null) m_HealthSlider.value = m_Health;

        if (m_Health <= 0)
            Die();
    }

    private void Die()
    {
        m_CurrentState = DrownState.Death;
         
        if (m_BlastEffectPrefab != null)
        {
            GameObject fx = Instantiate(m_BlastEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        Destroy(gameObject, 0.5f);
    }
}
