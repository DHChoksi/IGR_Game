using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 8f;

    void Start()
    {
        Destroy(gameObject,8f);
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) 
        {
            SpaceShipHealth enemy = other.gameObject.GetComponent<SpaceShipHealth>();
            if (enemy != null) 
            {
                enemy.ApplyDamage(50);
                Destroy(gameObject, 0.05f);
            }
        }
    }

}
 