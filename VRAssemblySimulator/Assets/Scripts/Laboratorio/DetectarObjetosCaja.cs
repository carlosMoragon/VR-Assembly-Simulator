using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarObjetosCaja: MonoBehaviour
{
    public GameObject puerta;
    public GameObject objetoConScriptDetectarEtiqueta;

    private Vector3 posicionInicialLocal = new Vector3(-0.18385f, 0.6f, 0f);
    private Vector3 posicionFinalLocal = new Vector3(-0.18385f, 1.135f, 0f);

    private float duracionMovimiento = 2f;

    private string tagObjetivo = null;

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
        tagObjetivo = objetoConScriptDetectarEtiqueta.GetComponent<DetectorEtiquetas>().ObtenerTagObjeto();

        if (tagObjetivo != null)
        {
            Debug.Log("Tag detectado: " + tagObjetivo);
            objetoConScriptDetectarEtiqueta.GetComponent<DetectorEtiquetas>().VolverAPosicion(other.tag);
            StartCoroutine(SecuenciaCerrarYReabrirPuerta(other));
        }
        else
        {
            Debug.Log("Esperando que se detecte el tag en el otro objeto...");
        }
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