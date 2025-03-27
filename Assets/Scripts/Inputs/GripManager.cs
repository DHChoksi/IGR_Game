/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants.Constants;

public class GripManager : MonoBehaviour
{
    [Header("Grip Manager Settings")]
    [SerializeField] 
    private LR_Device m_CurrentDevice;

    [SerializeField] 
    private Transform m_ControllerTip;
    
    [SerializeField] 
    private LayerMask m_TrashLayer; 
    
    [SerializeField] 
    private LayerMask m_PlatformLayer; 

    [SerializeField]
    private GripAction m_CurrentGripAction = GripAction.None;

    public LR_Device CurrentLRDevice
    {
        get { return m_CurrentDevice; }
        private set { m_CurrentDevice = value; }
    }

    public static LR_Device _CurrentLRDevice
    {
        get => Instance ? Instance.m_CurrentDevice : LR_Device.None;
    }
    public GripAction CurrentGripAction
    {
        get { return m_CurrentGripAction; }
        private set { m_CurrentGripAction = value; }
    }

    public static GripManager Instance { get; private set; }

    public static GripAction _CurrentGripAction
    {
        get => Instance ? Instance.m_CurrentGripAction : GripAction.None;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    private void FixedUpdate()
    {
        
        DetectTarget();
    }

    private void DetectTarget()
    {
        RaycastHit hit;

        if (Physics.Raycast(m_ControllerTip.position, m_ControllerTip.forward, out hit, MAX_HIT_DETECT_DISTANCE))
        {
            if ((m_TrashLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
            {
                m_CurrentGripAction = GripAction.HyperHook;
            }
            else if ((m_PlatformLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
            {
                m_CurrentGripAction = GripAction.WebSwinging;
            }
            else
            {
                m_CurrentGripAction = GripAction.None;
            }
        }
        else
        {
            m_CurrentGripAction = GripAction.None;  
        }
    }
}
*/