using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivarGravedadPuerta : MonoBehaviour
{
    public GameObject[] objetos = new GameObject[4];
    private Rigidbody rb;
    private bool gravedadActivada = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating(nameof(VerificarEstados), 0f, 0.1f);
    }

    void VerificarEstados()
    {
        if (gravedadActivada) return;

        foreach (GameObject obj in objetos)
        {
            EstadoTornillo estado = obj.GetComponent<EstadoTornillo>();
            if (estado != null && (estado.posicion || estado.atornillado))
            {
                return;
            }
        }

        rb.useGravity = true;

        BoxCollider col = GetComponent<BoxCollider>();
        if (col != null)
        {
            col.enabled = false;
        }

        if (GetComponent<XRGrabInteractable>() == null)
        {
            gameObject.AddComponent<XRGrabInteractable>();
        }

        gravedadActivada = true;
    }
}
