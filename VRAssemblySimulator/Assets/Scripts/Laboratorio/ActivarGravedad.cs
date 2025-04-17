using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarGravedad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            rb.useGravity = true;
        }
    }
}
