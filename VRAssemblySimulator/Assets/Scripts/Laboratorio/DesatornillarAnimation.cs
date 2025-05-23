using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DesatornillarAnimation : MonoBehaviour
{
    public Transform punta;
    public float duracion = 2f;
    public float distancia = 0.2f;
    private bool enProceso = false;

    private void OnTriggerEnter(Collider other)
    {
        if (enProceso) return;
        if (!other.CompareTag("tornillo")) return;

        Rigidbody rbDestornillador = GetComponent<Rigidbody>();
        if (rbDestornillador != null)
            rbDestornillador.useGravity = false;

        XRGrabInteractable grab = GetComponent<XRGrabInteractable>();
        if (grab != null)
            Destroy(grab);

        EstadoTornillo estado = other.GetComponent<EstadoTornillo>();
        if (estado == null) return;

        transform.rotation = Quaternion.Euler(0, 270, 0);
        Vector3 nuevaPosicion = new Vector3(other.transform.position.x, other.transform.position.y, transform.position.z);
        transform.position = nuevaPosicion;

        Vector3 direccion = (punta.position - other.transform.position).normalized;
        float distanciaActual = Vector3.Distance(punta.position, other.transform.position);
        float ajuste = 0.09f - distanciaActual;
        transform.position += direccion * ajuste;

        if (estado.posicion && estado.atornillado)
        {
            StartCoroutine(Desatornillar(other.transform, estado));
        }
        else if (estado.posicion && !estado.atornillado)
        {
            StartCoroutine(Atornillar(other.transform, estado));
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

        if (tornillo.GetComponent<XRGrabInteractable>() == null)
        {
            tornillo.gameObject.AddComponent<XRGrabInteractable>();
        }

        estado.posicion = false;
        estado.atornillado = false;

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

        estado.posicion = true;
        estado.atornillado = true;

        RestaurarDestornillador();
        enProceso = false;
    }

    private void RestaurarDestornillador()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.useGravity = true;

        if (GetComponent<XRGrabInteractable>() == null)
            gameObject.AddComponent<XRGrabInteractable>();
    }
}