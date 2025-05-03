using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClusterManager : MonoBehaviour
{
    [Header("Waypoints Path")]
    [SerializeField] private Transform[] clusterWaypoints;

    [Header("Enemy Group Root")]
    [SerializeField] private Transform enemyClusterRoot;  // Parent of all Drowns

    [Header("Player Reference")]
    [SerializeField] private Transform playerHead;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private float waypointThreshold = 0.5f;

    private int waypointIndex = 0;
    private Transform currentWaypoint;

    private void Start() 
    {
        playerHead = GameObject.FindGameObjectWithTag("Player").transform;
        if (clusterWaypoints.Length == 0 || enemyClusterRoot == null)
        {
            Debug.LogError("Missing waypoints or enemyClusterRoot reference.");
            return;
        }

        enemyClusterRoot.position = clusterWaypoints[0].position;
        currentWaypoint = clusterWaypoints[0];

        // Assign playerHead and clusterCenter to all children
        foreach (Transform child in enemyClusterRoot)
        {
            var ai = child.GetComponent<Enemy>();
            if (ai != null)
            {
                ai.SetPlayerHead(playerHead);
                ai.SetClusterParent(enemyClusterRoot);
            }
        }
    }

    private void Update()
    {
        if (clusterWaypoints.Length == 0 || enemyClusterRoot == null) return;

        // Move the entire cluster root
        enemyClusterRoot.position = Vector3.MoveTowards(
            enemyClusterRoot.position,
            currentWaypoint.position,
            moveSpeed * Time.deltaTime
        );

        // Rotate the cluster to face the waypoint
        Vector3 dir = currentWaypoint.position - enemyClusterRoot.position;
        if (dir.sqrMagnitude > 0.1f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            enemyClusterRoot.rotation = Quaternion.Slerp(
                enemyClusterRoot.rotation,
                lookRot,
                rotateSpeed * Time.deltaTime
            );
        }

        // Next waypoint logic
        if (Vector3.Distance(enemyClusterRoot.position, currentWaypoint.position) < waypointThreshold)
        {
            waypointIndex = (waypointIndex + 1) % clusterWaypoints.Length;
            currentWaypoint = clusterWaypoints[waypointIndex];
        }
    }
}
