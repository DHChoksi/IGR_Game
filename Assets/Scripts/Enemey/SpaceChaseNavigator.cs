
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Handles 3D space pathfinding and obstacle avoidance using raycasts (whiskers).
/// Chases a target while steering clear of obstacles.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SpaceChaseNavigator : MonoBehaviour
{
    [Header("Targeting")]
    public Transform target;
    public bool predictTarget = true;
    [Range(0f, 1f)] public float seekWeight = 0.7f;
    [Header("Speed & Turn")]
    public float maxSpeed = 30f;
    public float acceleration = 50f;
    public float maxAngularSpeed = 120f;
    public float slowRadius = 25f;
    [Header("Obstacle Avoidance (Raycasts)")]
    public float whiskerBaseLength = 15f;
    public float forwardBoost = 1.5f;
    public float clearance = 2f;
    public LayerMask obstacleMask = ~0;
    [Range(0f, 1f)] public float avoidWeight = 0.9f;
    public float whiskerAngle = 25f;
    public float diagonalAngle = 12.5f;
    [Range(1, 2)] public int whiskerRings = 2;
    [Header("Banking/Visuals")]
    public float bankAmount = 15f;
    public float bankSmoothing = 6f;
    [Header("Debug")]
    public bool drawWhiskers = true;
    public bool drawDesired = true;
    Rigidbody rb;
    float bank;

    [Header("Separation")]
    [Tooltip("How close before ships start repelling each other")]
    public float separationRadius = 12f;
    [Tooltip("How strong is the ship-ship separation force (0-1)")]
    [Range(0f, 1f)]
    public float separationWeight = 0.7f;

    public static List<SpaceChaseNavigator> allShips = new List<SpaceChaseNavigator>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.5f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        allShips.Add(this);
    }

    void OnDestroy()
    {
        allShips.Remove(this);
    }
    void FixedUpdate()
    {
        if (!target) return;

        Vector3 seekDir = GetSeekDirection();
        Vector3 avoidDir = GetAvoidanceDirection();
        Vector3 separateDir = GetSeparationDirection();

        // Blend: first avoid obstacles, then blend separation, then seek
        Vector3 blended = seekDir;

        if (avoidDir != Vector3.zero)
            blended = Vector3.Slerp(blended, avoidDir, avoidWeight);

        if (separateDir != Vector3.zero)
            blended = Vector3.Slerp(blended, separateDir, separationWeight);

        SteerAndThrust(blended.normalized);
        ApplyBanking(blended.normalized);
    }

    Vector3 GetSeekDirection()
    {
        Vector3 aim = target.position;
        if (predictTarget)
        {
            Vector3 tv = Vector3.zero;
            var trb = target.GetComponent<Rigidbody>();
            if (trb) tv = trb.linearVelocity;
            else tv = TargetVelocityEstimator.GetVelocity(target);
            float speed = rb.linearVelocity.magnitude + 0.1f;
            float dist = Vector3.Distance(transform.position, target.position);
            float lookAhead = Mathf.Clamp(dist / Mathf.Max(speed, 0.1f), 0.1f, 2.5f);
            aim += tv * lookAhead;
        }
        return (aim - transform.position).normalized;
    }
    Vector3 GetAvoidanceDirection()
    {
        Vector3 f = transform.forward;
        Vector3 u = transform.up;
        Vector3 r = transform.right;
        float speed = rb.linearVelocity.magnitude;
        float baseLen = whiskerBaseLength + speed;
        float forwardLen = baseLen * (1f + forwardBoost);
        bool forwardBlocked = Physics.SphereCast(transform.position, clearance, f, out RaycastHit fHit, forwardLen, obstacleMask);
        Vector3 bestDir = Vector3.zero;
        float bestScore = -Mathf.Infinity;
        void Score(Vector3 dir, float len)
        {
            float score = Physics.SphereCast(transform.position, clearance, dir, out RaycastHit hit, len, obstacleMask)
                ? (hit.distance / len)
                : 1f;
            score += Vector3.Dot(dir, f) * 0.25f;
            if (score > bestScore)
            {
                bestScore = score;
                bestDir = dir;
            }
        }
        Score(f, forwardLen);
        if (!forwardBlocked && bestScore > 1.1f) return Vector3.zero;
        Quaternion[] dirs = new Quaternion[]
        {
            Quaternion.AngleAxis(-whiskerAngle, u),
            Quaternion.AngleAxis(whiskerAngle, u),
            Quaternion.AngleAxis(-whiskerAngle, r),
            Quaternion.AngleAxis(whiskerAngle, r)
        };
        foreach (var q in dirs) Score(q * f, baseLen);
        if (whiskerRings >= 2)
        {
            Quaternion[] diags = new Quaternion[]
            {
                Quaternion.AngleAxis(-diagonalAngle, u) * Quaternion.AngleAxis(-diagonalAngle, r),
                Quaternion.AngleAxis(diagonalAngle, u) * Quaternion.AngleAxis(-diagonalAngle, r),
                Quaternion.AngleAxis(-diagonalAngle, u) * Quaternion.AngleAxis(diagonalAngle, r),
                Quaternion.AngleAxis(diagonalAngle, u) * Quaternion.AngleAxis(diagonalAngle, r)
            };
            foreach (var q in diags) Score(q * f, baseLen);
        }
        return bestDir == Vector3.zero ? f : bestDir;
    }
    void SteerAndThrust(Vector3 dir)
    {
        Quaternion targetRot = Quaternion.LookRotation(dir, transform.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, maxAngularSpeed * Time.fixedDeltaTime);
        float desiredSpeed = maxSpeed;
        if (slowRadius > 0f && target)
        {
            float dist = Vector3.Distance(transform.position, target.position);
            desiredSpeed = dist < slowRadius ? Mathf.Lerp(0f, maxSpeed, dist / slowRadius) : maxSpeed;
        }
        Vector3 desiredVel = transform.forward * desiredSpeed;
        Vector3 dv = desiredVel - rb.linearVelocity;
        Vector3 acc = Vector3.ClampMagnitude(dv, acceleration);
        rb.AddForce(acc, ForceMode.Acceleration);
    }
    void ApplyBanking(Vector3 dir)
    {
        Vector3 localDir = transform.InverseTransformDirection(dir);
        float desiredBank = Mathf.Clamp(-localDir.x * bankAmount, -bankAmount, bankAmount);
        bank = Mathf.Lerp(bank, desiredBank, 1f - Mathf.Exp(-bankSmoothing * Time.fixedDeltaTime));
    }

    Vector3 GetSeparationDirection()
    {
        Vector3 separation = Vector3.zero;
        int count = 0;
        foreach (var other in allShips)
        {
            if (other == this) continue;
            float dist = Vector3.Distance(transform.position, other.transform.position);
            if (dist < separationRadius && dist > 0.01f)
            {
                separation += (transform.position - other.transform.position) / dist;
                count++;
            }
        }
        if (count > 0)
            separation = (separation / count).normalized;
        return separation;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!drawWhiskers && !drawDesired) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, clearance);
    }
#endif
}
/// <summary>
/// Lightweight velocity estimator for targets without Rigidbody.
/// </summary>
public class TargetVelocityEstimator : MonoBehaviour
{
    public Transform target;
    public Vector3 Velocity { get; private set; }
    Vector3 lastPos;
    void LateUpdate()
    {
        if (!target) return;
        Vector3 p = target.position;
        Velocity = (p - lastPos) / Mathf.Max(Time.deltaTime, 1e-6f);
        lastPos = p;
    }
    public static Vector3 GetVelocity(Transform t)
    {
        var holder = t.GetComponent<_VelocityHolder>();
        if (!holder) holder = t.gameObject.AddComponent<_VelocityHolder>();
        return holder.StepAndGetVelocity();
    }
    private class _VelocityHolder : MonoBehaviour
    {
        Vector3 last, vel;
        void OnEnable() => last = transform.position;
        public Vector3 StepAndGetVelocity()
        {
            Vector3 p = transform.position;
            vel = (p - last) / Mathf.Max(Time.deltaTime, 1e-6f);
            last = p;
            return vel;
        }
    }
}
