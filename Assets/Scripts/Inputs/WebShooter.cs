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
    private float m_SpringStrength = 4.5f;


    public RaycastHit raycastHit;

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
        Debug.Log("Start Swing");
    }
     
    private void OnDisable()
    {
        InputEvents.GripActionInputs -= OnGrip;
    }
   
    private void StartSwing()
    {
        if (raycastHit.collider == null)
            return;

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

    public bool IsGrappling()
    {
        return m_SpringJoint != null;
    }

    private void StopSwing()
    {
        raycastHit = new RaycastHit();
        Destroy(m_SpringJoint); 
    }
     
    void OnGrip(LR_Device lr_Device, ControlType controlType, ControlState controlState, float gripValue)
    {
        Debug.Log("Start Swing");
        if (m_CurrentDevice != lr_Device) 
        {
            return;
        }

        if (controlState == ControlState.Pressed)
        {
            StartSwing();
        }
        
        if (controlState == ControlState.NotPressed) 
        {
            StopSwing();
        }
    }
} 
 