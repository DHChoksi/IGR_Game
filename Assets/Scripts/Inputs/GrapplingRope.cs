using GogoGaga.OptimizedRopesAndCables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    private Vector3 m_CurrentGrapplePosition = Vector3.zero;    

    [SerializeField]
    private WebShooter m_WebShooter = null;

    [SerializeField]
    private Spring m_Spring = null;

    [SerializeField] 
    private float m_Strength = 0;

    [SerializeField]
    private float m_Damper = 0;

    [SerializeField] 
    private float m_Velocity = 0;

    [SerializeField]
    private Transform webShooterEnd;
    
    [SerializeField]
    private Transform webShooterStart;

    [SerializeField]
    private Rope rope;

    List<Vector3> points = new();

    private void Awake()
    {
        m_Spring = new Spring();
        m_Spring.SetTarget(0);
    }

    void LateUpdate()
    {
        DrawRope();
    }

    private void DrawRope()
    {
        if (!m_WebShooter.IsGrappling())
        {
            m_CurrentGrapplePosition = m_WebShooter._GunTip.position;
            rope.SetStartPoint(webShooterStart);
            m_Spring.Reset();
            points.Clear();
            
            rope.gameObject.SetActive(false);
            return;
        }

        if(points.Count == 0)
        {
            rope.gameObject.SetActive(true);
            m_Spring.SetVelocity(m_Velocity);
        }
        m_Spring.SetStrength(m_Strength);
        m_Spring.Update(Time.deltaTime);
        m_Spring.SetDamper(m_Damper);

        Vector3 grapplePoint = m_WebShooter._CurrentGrapplePosition;
        Vector3 gunTip = m_WebShooter._GunTip.position;     
        Vector3 quaternion = Quaternion.LookRotation((grapplePoint - gunTip).normalized) * Vector3.up;

        m_CurrentGrapplePosition = Vector3.Lerp(m_CurrentGrapplePosition, grapplePoint, Time.deltaTime * 12f);
        webShooterEnd.position = m_CurrentGrapplePosition;

        rope.SetEndPoint(webShooterEnd); 
    }
} 
