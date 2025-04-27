using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static Constants.Constants;

public class HyperHook : MonoBehaviour
{
    [Header("Hook Settings")]
    [SerializeField] 
    private float m_GrabingSpeed = 10f; 
    
    [SerializeField] 
    private Transform m_GrabingTip = null; 
    
    [SerializeField] 
    private LayerMask m_GrabbableLayer; 

    [SerializeField]
    private LR_Device m_CurrentDevice = LR_Device.None;

    
    private Transform m_GrabbedObject = null; 
    private Rigidbody m_GrabbedRigidbody = null;
    private LineRenderer m_WebLine;

    [SerializeField]
    private CrosshairController m_CrosshairController = null;

    private GripAction m_CurrentGripAction = GripAction.None;

    private void Awake()
    {
        m_WebLine = gameObject.AddComponent<LineRenderer>();
        m_WebLine.startWidth = 0.02f;
        m_WebLine.endWidth = 0.01f;
        m_WebLine.material = new Material(Shader.Find("Sprites/Default")); 
    }
    private void OnEnable()
    {
        InputEvents.GripActionInputs += OnGrip;
    }

    private void OnDisable()
    {
        InputEvents.GripActionInputs -= OnGrip;
    }

    private void Update()
    {
        m_CurrentGripAction = m_CurrentDevice == LR_Device.L_Device ? m_CrosshairController._LeftGripActionType : m_CrosshairController._RightGripActionType;

        if (m_GrabbedObject != null)
        {
            m_WebLine.SetPosition(0, m_GrabingTip.position);
            m_WebLine.SetPosition(1, m_GrabbedObject.position);
        }
        else
        {
            m_WebLine.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (m_GrabbedObject != null)
        {
            Vector3 direction = (m_GrabingTip.position - m_GrabbedObject.position).normalized;
            float distance = Vector3.Distance(m_GrabingTip.position, m_GrabbedObject.position);

            if (m_GrabbedRigidbody != null)
            { 
                m_GrabbedRigidbody.velocity = direction * m_GrabingSpeed;
            }

            if (distance < 1f)
            {
                Vector3 ObjectPosition = m_GrabingTip.transform.position;  
                m_GrabbedObject.SetParent(m_GrabingTip);
                m_GrabbedRigidbody.velocity = Vector3.zero;
                m_WebLine.enabled = false;
                Debug.Log("Comming here");
            }
        }
    }

    private void OnGrip(LR_Device lr_Device, ControlType controlType, ControlState controlState, float gripValue)
    { 
        //m_CurrentDevice != GripManager._CurrentLRDevice ||  
        if (m_CurrentDevice != lr_Device)// || m_GripActionType != GripAction.HyperHook)
        {
            return;  
        }  

       // Debug.Log("Hyper Hook " + m_GripActionType.ToString());// + " | " + GripManager._CurrentGripAction.ToString());

        if (controlState == ControlState.Pressed)// && m_CurrentGripAction == GripAction.HyperHook)// && m_GripActionType == GripManager._CurrentGripAction)
        { 
            TryGrabObject(); 
        }
        
        if (controlState == ControlState.NotPressed) 
        {
            ReleaseObject();
        }
    }

    private void TryGrabObject()
    {
        if (m_GrabbedObject != null) 
            return; 

        RaycastHit hit;
        if (Physics.Raycast(m_GrabingTip.position, m_GrabingTip.forward, out hit, TRASH_DETECT_DISTANCE, m_GrabbableLayer))
        {
            m_GrabbedObject = hit.transform;
            m_GrabbedRigidbody = m_GrabbedObject.GetComponent<Rigidbody>();

            if (m_GrabbedRigidbody != null)
            {
                m_GrabbedRigidbody.drag = 2f; 
            }

            m_WebLine.enabled = true; 
        }
    } 

    private void ReleaseObject()
    {
        if (m_GrabbedObject != null)
        {
            m_GrabbedObject.SetParent(null);

            if (m_GrabbedRigidbody != null)
            {
                m_GrabbedRigidbody.drag = 0f;

                Vector3 shootDirection = m_GrabingTip.forward;  
                float shootForce = m_GrabingSpeed * 2f;         

                m_GrabbedRigidbody.velocity = shootDirection * shootForce;
            }
            
            m_GrabbedObject = null;
            m_GrabbedRigidbody = null;
        }
    }
}
