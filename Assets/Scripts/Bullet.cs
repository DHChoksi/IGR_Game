using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{ 
    void Start()
    {
        StartCoroutine(SelfDestroy(8f));
    }
   
    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))  
        {
            StartCoroutine(SelfDestroy(0.1f)); 
        }
    }

    private IEnumerator SelfDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
 