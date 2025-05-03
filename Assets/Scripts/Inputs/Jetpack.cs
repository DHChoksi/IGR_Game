using System.Collections;
using UnityEngine;
using static Constants.Constants;

public class Jetpack : MonoBehaviour
{
    [Header("Jetpack Settings")]
    [SerializeField] 
    private float m_JetpackForce = 10f; 
    
    [SerializeField] 
    private float m_Acceleration = 5f;   
    
    [SerializeField] 
    private float m_Deceleration = 2f;   
    
    [SerializeField] 
    private Rigidbody m_PlayerRigidbody = null;  

    [Header("Controller Transforms")]
    [SerializeField] 
    private Transform m_LeftController;   
    
    [SerializeField]
    private Transform m_RightController;

    private Vector3 m_CurrentVelocity = Vector3.zero;
    private float m_LeftGripInput = 0f;
    private float m_RightGripInput = 0f;

    private void OnEnable()
    {
        InputEvents.GripActionInputs += OnGrip;
        InputEvents.SetJetpackInput += OnJetpackOverride;
    }

    private void OnDisable()
    {
        InputEvents.GripActionInputs -= OnGrip;
        InputEvents.SetJetpackInput -= OnJetpackOverride;
    }


    private void FixedUpdate()
    {
        Vector3 finalDirection = Vector3.zero;
        float totalGrip = m_LeftGripInput + m_RightGripInput;

        if (m_LeftGripInput > 0f)
        {
            finalDirection += -m_LeftController.forward * m_LeftGripInput;
        }
        if (m_RightGripInput > 0f)
        {
            finalDirection += -m_RightController.forward * m_RightGripInput;
        }

        if (totalGrip > 0f)
        {
            finalDirection.Normalize();  
            Vector3 targetVelocity = finalDirection * m_JetpackForce;
            m_CurrentVelocity = Vector3.Lerp(m_CurrentVelocity, targetVelocity, Time.fixedDeltaTime * m_Acceleration);
        }
        else
        {
            m_CurrentVelocity = Vector3.Lerp(m_CurrentVelocity, Vector3.zero, Time.fixedDeltaTime * m_Deceleration);
        }

        m_PlayerRigidbody.velocity = m_CurrentVelocity; 
    }

    private void OnGrip(LR_Device lr_Device, ControlType controlType, ControlState controlState, float gripValue)
    {
        if (lr_Device == LR_Device.L_Device)
        {
            m_LeftGripInput = gripValue;
        }
        else if (lr_Device == LR_Device.R_Device)
        {
            m_RightGripInput = gripValue;
        }
    }

    private void OnJetpackOverride(LR_Device device, float gripValue)
    {
        if (device == LR_Device.L_Device)
        {
            m_LeftGripInput = gripValue;
        }
        else if (device == LR_Device.R_Device)
        {
            m_RightGripInput = gripValue;
        }
    }

}
