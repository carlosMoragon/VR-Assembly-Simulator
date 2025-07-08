using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorEtiquetas : MonoBehaviour
{
    private Vector3 posicionRelativaInicial = new Vector3(0.00013f, 0.0273f, -0.1993f);
    private Vector3 posicionRelativaFinal = new Vector3(0.00013f, 0.0273f, -0.5868f);
    private float duracion = 2.0f;

    private string tagObjetoDetectado = null;
    private Collider objetoEntrado = null;

    private HashSet<string> etiquetasValidas = new HashSet<string> { "DiscoDuro", "CPU", "Fuente", "GPU", "PlacaBase", "RAM", "Refrigeracion", "UAlmacenamiento", "Ventilador" };

    private List<string> tagsValidos = new List<string> { "Fuente", "RAM", "DiscoDuro", "PlacaBase", "Ventilador", "Refrigeracion", "GPU" };

    void Start()
    {
        transform.localPosition = posicionRelativaInicial;
        StartCoroutine(MoverALaPosicion(posicionRelativaFinal, duracion));
    }

    IEnumerator MoverALaPosicion(Vector3 destinoRelativo, float tiempo)
    {
        Vector3 inicioRelativo = transform.localPosition;
        float tiempoTranscurrido = 0;

        while (tiempoTranscurrido < tiempo)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / tiempo;

            transform.localPosition = Vector3.Lerp(inicioRelativo, destinoRelativo, t);

            yield return null;
        }

        transform.localPosition = destinoRelativo;
    }

    public void VolverAPosicion(string tag)
    {
        StartCoroutine(MoverALaPosicion(posicionRelativaInicial, duracion));
        StartCoroutine(EsperarYComprobar(tag));
    }

    private IEnumerator EsperarYComprobar(string tag)
    {
        yield return StartCoroutine(MoverALaPosicion(posicionRelativaInicial, duracion));
        yield return new WaitForSeconds(1f);

        if (objetoEntrado != null && objetoEntrado.tag == tag)
        {
            Destroy(objetoEntrado.gameObject);
        }

        StartCoroutine(MoverALaPosicion(posicionRelativaFinal, duracion));
    }

    public string ObtenerTagObjeto()
    {
        return tagObjetoDetectado;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tagsValidos.Contains(other.tag))
            tagObjetoDetectado = other.tag;
            objetoEntrado = other;

        if (etiquetasValidas.Contains(other.tag))
        {
            other.transform.position = transform.position;
            other.transform.rotation = Quaternion.Euler(270, 180, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == objetoEntrado)
        {
            tagObjetoDetectado = null;
            objetoEntrado = null;
        }
    }
}