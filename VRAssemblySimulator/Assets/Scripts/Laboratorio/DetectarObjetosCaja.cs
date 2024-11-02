using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarObjetosCaja: MonoBehaviour
{
    public GameObject puerta;

    private Vector3 posicionInicialLocal = new Vector3(-0.18385f, 0f, 0.3f);
    private Vector3 posicionFinalLocal = new Vector3(-0.18385f, 0f, 0.881f);

    private float duracionMovimiento = 2f;

    private string tagObjetivo = "TarjetaGrafica";

    void Start()
    {
        StartCoroutine(MoverPuerta(posicionInicialLocal, posicionFinalLocal, duracionMovimiento));
    }

    private IEnumerator MoverPuerta(Vector3 inicio, Vector3 fin, float duracion)
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;

            float t = tiempoTranscurrido / duracion;

            puerta.transform.localPosition = Vector3.Lerp(inicio, fin, t);

            yield return null;
        }

        puerta.transform.localPosition = fin;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Objeto detectado");

        StartCoroutine(SecuenciaCerrarYReabrirPuerta(other));
    }

    private IEnumerator SecuenciaCerrarYReabrirPuerta(Collider other)
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(MoverPuerta(posicionFinalLocal, posicionInicialLocal, duracionMovimiento));

        if (other.CompareTag(tagObjetivo))
        {
            Debug.Log("Correcto");
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log("Incorrecto");
        }
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(MoverPuerta(posicionInicialLocal, posicionFinalLocal, duracionMovimiento));
    }
}