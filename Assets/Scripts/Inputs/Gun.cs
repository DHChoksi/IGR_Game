using System;
using System.Collections;
using UnityEngine;
using static Constants.Constants;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]

    [SerializeField]
    private GameObject m_Bullet;

    [SerializeField]
    private Transform m_MuzzleTransform;

    [SerializeField]
    private float m_BulletSpeed = 20f;

    [SerializeField]
    private float m_FireRate = 0.2f;

    [SerializeField]
    private LR_Device m_Device = LR_Device.None;

    private bool m_CanShoot = true;

    float waitTime = 0;

    private void OnEnable()
    {
        InputEvents.TriggerActionInputs += OnTrigger;
    }

    private void OnDisable()
    {
        InputEvents.TriggerActionInputs -= OnTrigger;
    }

    private void OnTrigger(LR_Device lr_Device, ControlType controlType, ControlState controlState)
    {
        if (lr_Device != m_Device)
            return;

        if (controlType == ControlType.Trigger && controlState == ControlState.Pressed && m_CanShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {

        if (m_Bullet != null && m_MuzzleTransform != null)
        {
            // Spawn the bullet with correct position and rotation immediately
            GameObject bullet = Instantiate(m_Bullet, m_MuzzleTransform.position, m_MuzzleTransform.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = bullet.transform.forward * m_BulletSpeed;
            }

            // Optional: Add VFX or SFX
            /*
            if (m_MuzzleFlash != null)
            {
                m_MuzzleFlash.Play();
            }

            if (m_GunShotSound != null)
            {
                m_GunShotSound.Play();
            }
            */

            //StartCoroutine(FireCooldown());
        }
    }

    //private IEnumerator FireCooldown()
    //{
    //    m_CanShoot = false;
    //    yield return new WaitForSeconds(m_FireRate);
    //    m_CanShoot = true;
    //}
}
