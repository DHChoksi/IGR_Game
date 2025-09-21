using UnityEngine;
using static Constants.Constants;

public class WebShooter : MonoBehaviour
{
    [SerializeField] private Transform m_GunTip = null;
    [SerializeField] private Transform m_Player = null;
    [SerializeField] private LR_Device m_CurrentDevice = LR_Device.None;

    private Vector3 m_SwingPoint = Vector3.zero;
    private SpringJoint m_SpringJoint = null;
    private Rigidbody m_PlayerRb;

    [Header("Pull Settings")]
    [Tooltip("Force applied to pull player toward grapple point.")]
    [SerializeField] private float m_PullForce = 20f;

    [Tooltip("Clamp max speed towards grapple point.")]
    [SerializeField] private float m_MaxPullSpeed = 15f;

    [Header("Particles")]
    [Tooltip("Particle system that plays while pulling.")]
    [SerializeField] private ParticleSystem m_PullParticles;

    [Tooltip("Minimum velocity along rope direction to keep particles playing.")]
    [SerializeField] private float m_MinVelocityThreshold = 1f;

    public RaycastHit raycastHit;

    private bool m_GripHeld = false;

    public Vector3 _CurrentGrapplePosition => m_SwingPoint;
    public Transform _GunTip => m_GunTip;

    private void OnEnable() => InputEvents.GripActionInputs += OnGrip;
    private void OnDisable()
    {
        InputEvents.GripActionInputs -= OnGrip;
        StopSwing();
    }

    private void Awake()
    {
        if (m_Player != null)
            m_PlayerRb = m_Player.GetComponent<Rigidbody>();
    }

    private void StartSwing()
    {
        if (raycastHit.collider == null || m_SpringJoint != null)
            return;

        m_SwingPoint = raycastHit.point;

        // Optional: keep SpringJoint for anchor reference (not rope length)
        m_SpringJoint = m_Player.gameObject.AddComponent<SpringJoint>();
        m_SpringJoint.autoConfigureConnectedAnchor = false;
        m_SpringJoint.connectedAnchor = m_SwingPoint;
        m_SpringJoint.maxDistance = 0f;
        m_SpringJoint.minDistance = 0f;
        m_SpringJoint.spring = 0f;
        m_SpringJoint.damper = 0f;
        m_SpringJoint.massScale = 1f;

        // Start particle system immediately
        if (m_PullParticles != null && !m_PullParticles.isPlaying)
            m_PullParticles.Play();
    }

    public bool IsGrappling() => m_SpringJoint != null;

    private void StopSwing()
    {
        raycastHit = new RaycastHit();
        if (m_SpringJoint != null)
        {
            Destroy(m_SpringJoint);
            m_SpringJoint = null;
        }

        // Stop particle system
        if (m_PullParticles != null && m_PullParticles.isPlaying)
            m_PullParticles.Stop();
    }

    private void Update()
    {
        if (m_SpringJoint == null || m_PlayerRb == null) return;

        Vector3 toPoint = m_SwingPoint - m_Player.position;
        Vector3 pullDir = toPoint.normalized;

        // Velocity along rope direction only
        float velAlongPull = Vector3.Dot(m_PlayerRb.linearVelocity, pullDir);

        // Remove sideways velocity
        m_PlayerRb.linearVelocity = pullDir * velAlongPull;

        // Apply pulling force if under max speed
        if (velAlongPull < m_MaxPullSpeed)
        {
            m_PlayerRb.AddForce(pullDir * m_PullForce, ForceMode.Acceleration);

            // Clamp after applying force
            velAlongPull = Vector3.Dot(m_PlayerRb.linearVelocity, pullDir);
            if (velAlongPull > m_MaxPullSpeed)
            {
                m_PlayerRb.linearVelocity = pullDir * m_MaxPullSpeed;
            }
        }
        else
        {
            // Already at or above max → lock to max speed
            m_PlayerRb.linearVelocity = pullDir * m_MaxPullSpeed;
        }

        // --- Particle System Control ---
        if (m_PullParticles != null)
        {
            if (velAlongPull > m_MinVelocityThreshold)
            {
                if (!m_PullParticles.isPlaying)
                    m_PullParticles.Play();
            }
            else
            {
                if (m_PullParticles.isPlaying)
                    m_PullParticles.Stop();
            }
        }
    }

    private void OnGrip(LR_Device lr_Device, ControlType controlType, ControlState controlState, float gripValue)
    {
        if (m_CurrentDevice != lr_Device)
            return;

        if (controlState == ControlState.Pressed)
        {
            if (!m_GripHeld)
            {
                m_GripHeld = true;
                StartSwing();
            }
        }
        else if (controlState == ControlState.NotPressed)
        {
            m_GripHeld = false;
            StopSwing();
        }
    }
}
