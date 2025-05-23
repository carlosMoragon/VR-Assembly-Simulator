using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarGravedad : MonoBehaviour
{
    private HashSet<string> etiquetasValidas = new HashSet<string>
    {
        "DiscoDuro", "CPU", "Fuente", "GPU", "PlacaBase",
        "RAM", "Refrigeracion", "UAlmacenamiento", "Ventilador"
    };

    private void OnTriggerEnter(Collider other)
    {
        if (etiquetasValidas.Contains(other.tag))
        {
            Rigidbody rb = other.attachedRigidbody;

            if (rb != null)
            {
                rb.useGravity = true;
            }
        }
    }
}
