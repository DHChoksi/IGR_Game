using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCollider : MonoBehaviour
{
    [SerializeField] 
    private Collider m_RangeCollider;

    private void Start()
    {
        m_RangeCollider = GetComponent<Collider>();    
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 objectPosiiton = other.gameObject.transform.position;
        CalculateDistance(objectPosiiton);
    }

    private void CalculateDistance(Vector3 posiiton)
    {
        float distance = Vector3.Distance(transform.position, posiiton);
        float minDistance = 2.5f;

        if (distance <= minDistance)
        {
           // Debug.Log("Exposed" + distance);
        }
    }

}
