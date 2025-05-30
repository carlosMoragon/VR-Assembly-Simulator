using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AtraerTornillo : MonoBehaviour
{
    public bool libre = true;
    public Transform tornilloAgarrado;

    private void OnTriggerEnter(Collider other)
    {
        if (libre && other.CompareTag("tornillo"))
        {
            other.isTrigger = true;
            Vector3 nuevaPos = transform.position + new Vector3(0f, 0f, -0.015f);
            other.transform.position = nuevaPos;
            libre = false;

            XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                Destroy(grab);
            }

            EstadoTornillo estado = other.GetComponent<EstadoTornillo>();
            if (estado != null)
            {
                estado.posicion = true;
            }

            tornilloAgarrado = other.transform;
            tornilloAgarrado.parent = transform;
        }
    }
}
