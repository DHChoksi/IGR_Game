using DG.Tweening;
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
    private Transform gunModel;

    [SerializeField]
    public Vector3 recoilValue;

    [SerializeField]
    private Transform m_MuzzleTransform;

    [SerializeField]
    private float m_BulletSpeed = 20f;

    [SerializeField]
    private float m_FireRate = 0.2f;

    [SerializeField]
    private LR_Device m_Device = LR_Device.None;

    [SerializeField]
    private ParticleSystem muzzleFlash;

    private bool m_CanShoot = true;
    public CrosshairTargeting crosshairTargeting;
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

            if(!DOTween.IsTweening(gunModel))
            {
                gunModel.DOLocalRotate(recoilValue, 0.1f, RotateMode.LocalAxisAdd).OnComplete(() => { gunModel.localEulerAngles = Vector3.zero; });
            }

            GameObject bullet = Instantiate(m_Bullet, m_MuzzleTransform.position, m_MuzzleTransform.rotation);
            muzzleFlash.Play();
            AudioManager.Instance.PlaySFX(SFXType.GunShoot, 0.5f);
            bullet.transform.forward = crosshairTargeting.camRay.direction;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = bullet.transform.forward * m_BulletSpeed;
            }
        }
    }

}
