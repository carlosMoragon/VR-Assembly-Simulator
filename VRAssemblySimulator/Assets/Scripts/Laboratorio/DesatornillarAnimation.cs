using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesatornillarAnimation : MonoBehaviour
{
    public Transform punta;
    public float duracionDesatornillado = 2f;
    public float distanciaAtras = 0.2f;
    private bool desatornillando = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!desatornillando && other.CompareTag("tornillo"))
        {
            Vector3 nuevaPosicion = new Vector3(other.transform.position.x, other.transform.position.y, transform.position.z);
            transform.position = nuevaPosicion;

            Vector3 direccion = (punta.position - other.transform.position).normalized;
            float distanciaActual = Vector3.Distance(punta.position, other.transform.position);
            float ajuste = 0.09f - distanciaActual;
            transform.position += direccion * ajuste;
            StartCoroutine(Desatornillar(other.transform));
        }
    }

    private IEnumerator Desatornillar(Transform tornillo)
    {
        desatornillando = true;
        float tiempo = 0f;
        while (tiempo < duracionDesatornillado)
        {
            tiempo += Time.deltaTime;
            transform.Rotate(Vector3.right * 360 * Time.deltaTime);
            tornillo.Rotate(Vector3.back * 360 * Time.deltaTime);
            transform.position += transform.right * (distanciaAtras / duracionDesatornillado) * Time.deltaTime;
            tornillo.position += transform.right * (distanciaAtras / duracionDesatornillado) * Time.deltaTime;
            yield return null;
        }

        Destroy(tornillo.gameObject);
        desatornillando = false;
    }
}