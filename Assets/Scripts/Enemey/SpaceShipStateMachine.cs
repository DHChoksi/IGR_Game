using UnityEngine;
using System.Collections.Generic;

public class SpaceShipStateMachine : MonoBehaviour
{
    public enum ShipState { Idle, Patrol, Chase, Attack }

    [Header("References")]
    public SpaceChaseNavigator navigator;
    public Transform target;
    public SpaceShipCombat combat;

    [Header("Patrol Settings")]
    public List<Transform> patrolPoints = new List<Transform>();
    public float patrolPointTolerance = 10f;
    public float patrolWaitTime = 2f;

    [Header("Combat Settings")]
    public float attackRange = 150f;        // Distance to stop & shoot
    public float chaseResumeRange = 160f;   // Resume chase if target escapes

    private ShipState currentState = ShipState.Idle;
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private Rigidbody rb;

    void Start()
    {
        if (!navigator) navigator = GetComponent<SpaceChaseNavigator>();
        if (!combat) combat = GetComponent<SpaceShipCombat>();
        rb = GetComponent<Rigidbody>();

        if (target) SetState(ShipState.Chase);
        else if (patrolPoints.Count > 0) SetState(ShipState.Patrol);
        else SetState(ShipState.Idle);
    }

    void Update()
    {
        switch (currentState)
        {
            case ShipState.Idle: UpdateIdle(); break;
            case ShipState.Patrol: UpdatePatrol(); break;
            case ShipState.Chase: UpdateChase(); break;
            case ShipState.Attack: UpdateAttack(); break;
        }
    }

    void SetState(ShipState newState)
    {
        currentState = newState;
        waitTimer = 0f;

        if (newState == ShipState.Attack)
        {
            if (navigator) navigator.enabled = false;
            if (rb) rb.linearVelocity = Vector3.zero; // (Optional) stop immediately
        }
        else if (navigator)
        {
            navigator.enabled = true;
        }
    }

    void UpdateIdle()
    {
        if (navigator) navigator.enabled = false;
        if (target) SetState(ShipState.Chase);
        else if (patrolPoints.Count > 0) SetState(ShipState.Patrol);
    }

    void UpdatePatrol()
    {
        if (!navigator || patrolPoints.Count == 0) return;
        navigator.enabled = true;
        navigator.target = patrolPoints[currentPatrolIndex];

        float dist = Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position);
        if (dist < patrolPointTolerance)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                waitTimer = 0f;
            }
        }
        if (target) SetState(ShipState.Chase);
    }

    void UpdateChase()
    {
        if (!navigator) return;
        navigator.enabled = true;
        navigator.target = target;

        if (!target)
        {
            if (patrolPoints.Count > 0) SetState(ShipState.Patrol);
            else SetState(ShipState.Idle);
            return;
        }

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= attackRange)
        {
            SetState(ShipState.Attack);
        }
    }

    void UpdateAttack()
    {
        if (!target)
        {
            if (patrolPoints.Count > 0) SetState(ShipState.Patrol);
            else SetState(ShipState.Idle);
            return;
        }

        // Stay in place, face target, shoot
        FaceTarget();

        if (combat)
        {
            combat.TryShoot(target);
        }

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist > chaseResumeRange)
        {
            SetState(ShipState.Chase);
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, 200f * Time.deltaTime);
        }
    }
}
