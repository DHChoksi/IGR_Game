using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCollider : MonoBehaviour
{
    [SerializeField]
    private GameObject m_RangeEffect = null;
    
    private void OnTriggerEnter(Collider other)
    {
        Vector3 objectPosiiton = other.gameObject.transform.position;
        if (m_RangeEffect != null )
        {
            GameObject effect = Instantiate(m_RangeEffect, gameObject.transform.parent.position, Quaternion.identity);
            effect.transform.parent = transform;
            Destroy(transform.parent.gameObject, 1f); 
        }
    }


}
