using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClusterManager : MonoBehaviour
{
    [Header("Waypoints Path")]
    [SerializeField] private Transform[] m_ClusterWaypoints;

    [Header("Enemy Group Root")]
    [SerializeField] private Transform m_EnemyClusterRoot;  // Parent of all Drowns

    [Header("Player Reference")]
    [SerializeField] private Transform m_PlayerHead;

    [Header("Movement")]
    [SerializeField] private float m_MoveSpeed = 1.5f;
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private float waypointThreshold = 0.5f;

    private int waypointIndex = 0;
    private Transform currentWaypoint;

    private void Start() 
    {
        m_PlayerHead = GameObject.FindGameObjectWithTag("Player").transform;
        if (m_ClusterWaypoints.Length == 0 || m_EnemyClusterRoot == null)
        {
            Debug.LogError("Missing waypoints or enemyClusterRoot reference.");
            return;
        }

        m_EnemyClusterRoot.position = m_ClusterWaypoints[0].position;
        currentWaypoint = m_ClusterWaypoints[0];

        // Assign playerHead and clusterCenter to all children
        foreach (Transform child in m_EnemyClusterRoot)
        {
            var ai = child.GetComponent<Enemy>();
            if (ai != null)
            {
                ai.SetPlayerHead(m_PlayerHead);
                ai.SetClusterParent(m_EnemyClusterRoot);
            }
        }
    }

    private void Update()
    {
        if (m_ClusterWaypoints.Length == 0 || m_EnemyClusterRoot == null) return;

        // Move the entire cluster root
        m_EnemyClusterRoot.position = Vector3.MoveTowards(
            m_EnemyClusterRoot.position,
            currentWaypoint.position,
            m_MoveSpeed * Time.deltaTime
        );

        // Rotate the cluster to face the waypoint
        Vector3 dir = currentWaypoint.position - m_EnemyClusterRoot.position;
        if (dir.sqrMagnitude > 0.1f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            m_EnemyClusterRoot.rotation = Quaternion.Slerp(
                m_EnemyClusterRoot.rotation,
                lookRot,
                rotateSpeed * Time.deltaTime
            );
        }

        // Next waypoint logic
        if (Vector3.Distance(m_EnemyClusterRoot.position, currentWaypoint.position) < waypointThreshold)
        {
            waypointIndex = (waypointIndex + 1) % m_ClusterWaypoints.Length;
            currentWaypoint = m_ClusterWaypoints[waypointIndex];
        }
    }
}
