using UnityEngine;
using UnityEngine.UI;

public enum DrownState { Move, PlayerDetected, Attack, Death }

public class Enemy : MonoBehaviour
{
    public DrownState currentState = DrownState.Move;

    [Header("Combat")]
    public float m_PlayerDetectRadius = 10f;
    public float attackRange = 6f;
    public float attackCooldown = 2f;
    public float maxHealth = 2;
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;
    public GameObject blastEffectPrefab;

    [Header("UI")]
    public Slider healthSlider;

    private float health;
    private float attackTimer;

    private Transform playerHead;
    private Transform clusterCenter;
    private Vector3 localOffset;

    private Transform[] rocketTips;

    public void SetPlayerHead(Transform head)
    {
        playerHead = head;

        // Optional: Automatically find rocket tips
        rocketTips = new Transform[3];
        rocketTips[0] = GameObject.Find("RocketTip1")?.transform;
        rocketTips[1] = GameObject.Find("RocketTip2")?.transform;
        rocketTips[2] = GameObject.Find("RocketTip3")?.transform;
    }

    public void SetClusterParent(Transform parent, Vector3 offset)
    {
        clusterCenter = parent;
        localOffset = offset;
    }

    private void Start()
    {
        health = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
    }

    private void Update()
    {
        if (currentState == DrownState.Death || playerHead == null)
            return;

        float distToPlayer = Vector3.Distance(transform.position, playerHead.position);

        switch (currentState)
        {
            case DrownState.Move:
                if (clusterCenter != null)
                {
                    LookAt(clusterCenter.position + clusterCenter.forward * 10f); // Look ahead
                }
                 
                if (distToPlayer <= m_PlayerDetectRadius)
                    currentState = DrownState.PlayerDetected;
                break;


            case DrownState.PlayerDetected:
                LookAt(playerHead.position);
                MaintainClusterOffset();

                if (distToPlayer <= attackRange)
                {
                    Debug.Log("Attack !!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    currentState = DrownState.Attack;
                    attackTimer = attackCooldown;
                }
                break;

            case DrownState.Attack:
                LookAt(playerHead.position);
                attackTimer -= Time.deltaTime;

                if (attackTimer <= 0f)
                {
                    FireMissile();
                    attackTimer = attackCooldown;
                }

                if (distToPlayer > attackRange)
                    currentState = DrownState.PlayerDetected;
                break;
        }
    }

    private void MaintainClusterOffset()
    {
        if (clusterCenter == null) return;

        Vector3 targetPosition = clusterCenter.TransformPoint(localOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2f);
        LookAt(clusterCenter.position + clusterCenter.forward * 10f);
    }

    private void LookAt(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0;
        if (dir.magnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }
    }

    private void FireMissile()
    {
        if (missilePrefab == null || missileSpawnPoint == null || playerHead == null) 
            return;

        GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);

        Vector3 direction = (playerHead.position - missileSpawnPoint.position).normalized;
        missile.transform.rotation = Quaternion.LookRotation(direction);
         
        Rigidbody rb = missile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float missileSpeed = 300f; // Adjust as needed
            rb.velocity = direction * missileSpeed;
            rb.useGravity = false;   // Optional: depends on your missile type
        }

        Destroy(missile, 5f);
    }

    public void SetClusterParent(Transform parent)
    {
        clusterCenter = parent;
    }

    public void TakeDamage()
    {
        health = health - 0.5f;
        if (healthSlider != null) healthSlider.value = health;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        currentState = DrownState.Death;
         
        if (blastEffectPrefab != null)
        {
            GameObject fx = Instantiate(blastEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        Destroy(gameObject, 0.5f);
    }
}
