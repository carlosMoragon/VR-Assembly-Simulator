using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DesatornillarAnimation : MonoBehaviour
{
    public Transform punta;
    public float duracion = 2f;
    public float distancia = 0.03494f;
    private bool enProceso = false;

    public Transform[] opcionesMarcoPuerta;
    public AtraerTornillo atraeScript;

    private void OnTriggerEnter(Collider other)
    {
        if (enProceso) return;
        if (!other.CompareTag("tornillo")) return;

        EstadoTornillo estado = other.GetComponent<EstadoTornillo>();
        if (estado == null) return;

        if (estado.posicion)
        {
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }

            Rigidbody rbDestornillador = GetComponent<Rigidbody>();
            if (rbDestornillador != null)
                rbDestornillador.useGravity = false;

            XRGrabInteractable grab = GetComponent<XRGrabInteractable>();
            if (grab != null)
                Destroy(grab);

            transform.rotation = Quaternion.Euler(0, 270, 0);

            Vector3 nuevaPosicion = other.transform.position + new Vector3(0f, 0f, 0.24f);
            transform.position = nuevaPosicion;

            if (estado.atornillado)
            {
                StartCoroutine(Desatornillar(other.transform, estado));
            }
            else
            {
                StartCoroutine(Atornillar(other.transform, estado));
            }
        }
    }

    private IEnumerator Desatornillar(Transform tornillo, EstadoTornillo estado)
    {
        enProceso = true;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            transform.Rotate(Vector3.right * 360 * Time.deltaTime);
            tornillo.Rotate(Vector3.back * 360 * Time.deltaTime);
            transform.position += transform.right * (distancia / duracion) * Time.deltaTime;
            tornillo.position += transform.right * (distancia / duracion) * Time.deltaTime;
            yield return null;
        }

        Rigidbody rb = tornillo.GetComponent<Rigidbody>();
        if (rb == null) rb = tornillo.gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        Collider col = tornillo.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }

        if (tornillo.GetComponent<XRGrabInteractable>() == null)
            tornillo.gameObject.AddComponent<XRGrabInteractable>();

        if (atraeScript != null && atraeScript.tornilloAgarrado != null)
        {
            atraeScript.tornilloAgarrado.parent = null;
            atraeScript.tornilloAgarrado = null;
        }

        estado.posicion = false;
        estado.atornillado = false;

        tornillo.parent = null;

        RestaurarDestornillador();
        enProceso = false;
    }

    private IEnumerator Atornillar(Transform tornillo, EstadoTornillo estado)
    {
        enProceso = true;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            transform.Rotate(Vector3.left * 360 * Time.deltaTime);
            tornillo.Rotate(Vector3.forward * 360 * Time.deltaTime);
            transform.position -= transform.right * (distancia / duracion) * Time.deltaTime;
            tornillo.position -= transform.right * (distancia / duracion) * Time.deltaTime;
            yield return null;
        }

        Rigidbody rb = tornillo.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;
        }

        Collider col = tornillo.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }

        Transform padreCorrecto = EncontrarColliderPadre(tornillo.position);
        if (padreCorrecto != null)
        {
            tornillo.parent = padreCorrecto;
            if (atraeScript != null)
                atraeScript.tornilloAgarrado = tornillo;
        }

        estado.posicion = true;
        estado.atornillado = true;

        RestaurarDestornillador();
        enProceso = false;
    }

    private Transform EncontrarColliderPadre(Vector3 posTornillo)
    {
        foreach (Transform opcion in opcionesMarcoPuerta)
        {
            Collider col = opcion.GetComponent<Collider>();
            if (col != null && col.bounds.Contains(posTornillo))
                return opcion;
        }
        return null;
    }

    private void RestaurarDestornillador()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        if (rb != null)
            rb.useGravity = true;

        if (GetComponent<XRGrabInteractable>() == null)
            gameObject.AddComponent<XRGrabInteractable>();
    }
}
