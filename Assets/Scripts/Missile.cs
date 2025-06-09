using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ExplosionEffect = null;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (m_ExplosionEffect != null)
            {
                 GameObject explosion = Instantiate(m_ExplosionEffect, transform.position, Quaternion.identity);
                 explosion.transform.SetParent(this.gameObject.transform);
                 Destroy(gameObject, 1f);
            }
        }
    }
}
