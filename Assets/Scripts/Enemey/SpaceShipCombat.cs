
using UnityEngine;
/// <summary>
/// Handles combat logic for a spaceship: aiming, shooting, and projectile control.
/// Attach alongside SpaceShipStateMachine.
/// </summary>
public class SpaceShipCombat : MonoBehaviour
{
    [Header("Weapon Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float projectileSpeed = 80f;
    public float fireRange = 200f;
    public float aimTolerance = 10f;
    [Header("Effects")]
    public AudioSource fireSound;
    public ParticleSystem muzzleFlash;
    private float fireCooldown;
    void Update()
    {
        fireCooldown -= Time.deltaTime;
    }
    public void TryShoot(Transform target)
    {
        if (!target || !projectilePrefab || fireCooldown > 0f)
            return;
        Vector3 dir = (target.position - firePoint.position);
        float dist = dir.magnitude;
        if (dist > fireRange) return;
        dir.Normalize();
        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > aimTolerance) return;
        Shoot(dir);
    }
    void Shoot(Vector3 direction)
    {
        fireCooldown = 1f / fireRate;
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb) rb.linearVelocity = direction * projectileSpeed;
        if (fireSound) fireSound.Play();
        if (muzzleFlash) muzzleFlash.Play();
        Destroy(proj, 8f);
    }
}
