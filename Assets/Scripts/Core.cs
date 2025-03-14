using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Trash"))
        {
            Trash trashScript = other.GetComponent<Trash>();
             
            if (trashScript != null)
            {
                trashScript.AnimateAndDisable(transform.position);
            }
        }
    }
}