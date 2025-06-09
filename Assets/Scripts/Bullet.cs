using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 8f;

    void Start()
    {
        StartCoroutine(SelfDestroy(destroyDelay));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) 
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null) 
            {
                enemy.TakeDamage();
            }

            Destroy(gameObject, 0.05f); 
        }
    }

    private IEnumerator SelfDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
 