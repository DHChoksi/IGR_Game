using Unity.VisualScripting;
using UnityEngine;
using static Constants.Constants;

public class WebShooter : MonoBehaviour
{ 
    [SerializeField]
    private Transform m_GunTip = null;

    [SerializeField]
    private Transform m_Player = null;

    [SerializeField] 
    private LayerMask m_LayerMask;

    [SerializeField]    
    private LR_Device m_CurrentDevice = LR_Device.None; 

    private Vector3 m_SwingPoint = Vector3.zero;
    private SpringJoint m_SpringJoint = null;

    [SerializeField]
    private CrosshairController m_CrosshairController = null;   

    [SerializeField]
    private float m_SpringStrength = 4.5f;

    private GripAction m_CurrentGripAction = GripAction.None;
    public Vector3 _CurrentGrapplePosition 
    {
        get { return m_SwingPoint; }
        private set { m_SwingPoint = value;}
    }

    public Transform _GunTip
    {
        get { return m_GunTip; }
        private set { m_GunTip = value; }
    }

    private void OnEnable()
    { 
        InputEvents.GripActionInputs += OnGrip;
    }
     
    private void OnDisable()
    {
        InputEvents.GripActionInputs -= OnGrip;
    }

    private GripAction m_GripAction = GripAction.None;
   
    private void StartSwing()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(m_GunTip.position, m_GunTip.forward, out raycastHit, PLATFORM_DETECT_DISTANCE, m_LayerMask))
        {
            m_SwingPoint = raycastHit.point;
            m_SpringJoint = m_Player.AddComponent<SpringJoint>();
            m_SpringJoint.autoConfigureConnectedAnchor = false;
            m_SpringJoint.connectedAnchor = m_SwingPoint;

            float distanceFromPoint = Vector3.Distance(m_Player.position, m_SwingPoint);

            m_SpringJoint.maxDistance = distanceFromPoint * 0.9f;
            m_SpringJoint.minDistance = distanceFromPoint * 0.01f;

            m_SpringJoint.spring = m_SpringStrength;
            m_SpringJoint.massScale = 10f;
        }
    }

    private void Update()
    {
        m_CurrentGripAction = m_CurrentDevice == LR_Device.L_Device ? m_CrosshairController._LeftGripActionType : m_CrosshairController._RightGripActionType; 
    }

    public bool IsGrappling()
    {
        return m_SpringJoint != null;
    }

    private void StopSwing()
    {
        Destroy(m_SpringJoint); 
    }
     
    void OnGrip(LR_Device lr_Device, ControlType controlType, ControlState controlState, float gripValue)
    {
        //m_GripActionType != GripManager._CurrentGripAction || m_CurrentDecive != GripManager._CurrentLRDevice ||
        if (m_CurrentDevice != lr_Device) 
        {
            return;
        }

       // Debug.Log("Web Swinging " + m_GripActionType.ToString());// + " | " + GripManager._CurrentGripAction.ToString());
    
        if (controlState == ControlState.Pressed)// && m_CurrentGripAction == GripAction.WebSwinging)
        {
            StartSwing();
        }
        
        if (controlState == ControlState.NotPressed) 
        {
            StopSwing();
        }
    }
} 
 