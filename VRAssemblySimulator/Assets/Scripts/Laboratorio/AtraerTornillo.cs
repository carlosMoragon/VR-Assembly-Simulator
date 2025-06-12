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
        if (libre && other.CompareTag("tornillo") )
        {
            EstadoTornillo estado = other.GetComponent<EstadoTornillo>();

            if (estado.atornillado)
            {
                Vector3 posNormal = transform.position + new Vector3(0f, 0f, -0.048f);
                other.transform.position = posNormal;
                libre = false;
            }
            else
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

                if (estado != null)
                {
                    estado.posicion = true;
                }
            }

            tornilloAgarrado = other.transform;
            tornilloAgarrado.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (tornilloAgarrado != null && other.transform == tornilloAgarrado)
        {
            EstadoTornillo estado = tornilloAgarrado.GetComponent<EstadoTornillo>();
            if (estado != null)
            {
                estado.posicion = false;
            }

            tornilloAgarrado.parent = null;
            tornilloAgarrado = null;
            libre = true;
        }
    }
}