using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect_object : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el tag del objeto que entra es igual al tag del objeto que contiene este script
        if (other.gameObject.tag == this.gameObject.tag)
        {
            Debug.Log("FUNCIONA");
            other.transform.SetParent(transform);
            other.transform.localPosition = Vector3.zero;

            // Desactiva el Rigidbody del objeto
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            else
            {
                Debug.LogWarning("No se encontró un Rigidbody en " + other.gameObject.name);
            }
        }
        else
        {
            Debug.Log("Los tags NO coinciden. No se realiza ninguna acción.");
        }
    }
    
}
