
using UnityEngine;
public class Projectile : MonoBehaviour
{
    public float damage = 25f;
    public float lifetime = 8f;
    public GameObject impactEffect;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.transform.CompareTag("Player"))
        {
            if (impactEffect)
            {
                Destroy(Instantiate(impactEffect, transform.position, Quaternion.identity), 1f);
                Destroy(gameObject);
            }
        }
        //var hp = col.gameObject.GetComponent<SpaceShipHealth>();
        
        //if (hp)
        //    hp.ApplyDamage(damage);
        
    }
}
