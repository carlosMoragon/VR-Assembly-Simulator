using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DetectarObjetosCaja: MonoBehaviour
{
    public GameObject puerta;
    public GameObject objetoConScriptDetectarEtiqueta;

    private Vector3 posicionInicialLocal = new Vector3(-0.18385f, 0.6f, 0f);
    private Vector3 posicionFinalLocal = new Vector3(-0.18385f, 1.135f, 0f);

    private float duracionMovimiento = 2f;

    private string tagObjetivo = null;
    private Collider objetoEnCaja = null;
    private bool secuenciaEnCurso = false;

    private List<string> tagsValidos = new List<string> { "Fuente", "RAM", "DiscoDuro", "PlacaBase", "Ventilador", "Refrigeracion", "GPU" };

    string escenaActual = null;

    void Start()
    {
        escenaActual = SceneManager.GetActiveScene().name;

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
        if (tagsValidos.Contains(other.tag))
            objetoEnCaja = other;

        tagObjetivo = objetoConScriptDetectarEtiqueta.GetComponent<DetectorEtiquetas>().ObtenerTagObjeto();

        if (!secuenciaEnCurso)
            StartCoroutine(EsperarYProcesar());
    }

    private IEnumerator EsperarYProcesar()
    {
        secuenciaEnCurso = true;

        while (objetoEnCaja == null || string.IsNullOrEmpty(tagObjetivo))
        {
            tagObjetivo = objetoConScriptDetectarEtiqueta.GetComponent<DetectorEtiquetas>().ObtenerTagObjeto();
            yield return null;
        }

        objetoConScriptDetectarEtiqueta.GetComponent<DetectorEtiquetas>().VolverAPosicion(objetoEnCaja.tag);
        yield return StartCoroutine(SecuenciaCerrarYReabrirPuerta(objetoEnCaja));

        objetoEnCaja = null;
        secuenciaEnCurso = false;
    }

    private IEnumerator SecuenciaCerrarYReabrirPuerta(Collider other)
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(MoverPuerta(posicionFinalLocal, posicionInicialLocal, duracionMovimiento));

        if (other != null && other.CompareTag(tagObjetivo))
        {
            if (escenaActual != "Practice")
                GameManager.instance.SumarAcierto();
            Destroy(other.gameObject);
        }
        else
        {
            if (escenaActual != "Practice")
                GameManager.instance.SumarFallo();
        }

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(MoverPuerta(posicionInicialLocal, posicionFinalLocal, duracionMovimiento));
    }
}