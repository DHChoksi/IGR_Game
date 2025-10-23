
using UnityEngine;
using System.Collections.Generic;
public class SpaceFleetManager : MonoBehaviour
{
    [Header("Fleet Settings")]
    public GameObject shipPrefab;
    public int shipCount = 5;
    public float spawnRadius = 100f;
    public Transform target;
    [Header("Patrol Settings")]
    public List<Transform> globalPatrolPoints = new List<Transform>();
    private readonly List<SpaceShipStateMachine> fleet = new List<SpaceShipStateMachine>();
    void Start()
    {
        if (!shipPrefab)
        {
            Debug.LogWarning("No ship prefab assigned.");
            return;
        }
        for (int i = 0; i < shipCount; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Quaternion rot = Quaternion.LookRotation(Random.onUnitSphere);
            GameObject ship = Instantiate(shipPrefab, pos, rot);
            var sm = ship.GetComponent<SpaceShipStateMachine>();
            if (sm)
            {
                sm.target = target;
                sm.patrolPoints = globalPatrolPoints;
                fleet.Add(sm);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
