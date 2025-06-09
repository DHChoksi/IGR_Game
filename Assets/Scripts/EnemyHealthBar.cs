using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private Transform m_Camera;

    private void Start()
    {
        m_Camera = Camera.main.transform;
    }
    private void LateUpdate()
    {
        transform.LookAt(m_Camera);
    }
}
