using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshPro tiempoText;
    public TextMeshPro aciertosText;
    public TextMeshPro fallosText;

    [SerializeField] private GameObject[] piezasParaAcertar;

    private int aciertos = 0;
    private int totalPiezas;
    private int fallos = 0;
    private int maxFallos = 3;
    private float tiempoRestante = 60f;
    private bool contandoTiempo = true;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        totalPiezas = piezasParaAcertar.Length;
        aciertosText.text = "Aciertos:\n" + aciertos + "/" + totalPiezas;
        fallosText.text = "Fallos:\n" + fallos + "/" + maxFallos;
    }

    private void Update()
    {
        if (contandoTiempo && tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            tiempoText.text = "Tiempo restante:\n" + Mathf.Ceil(tiempoRestante) + " segundos";
        }
        else if (contandoTiempo && tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            contandoTiempo = false;
            tiempoText.text = "Se acabó el tiempo";
            RevisarCondicionesFinDeJuego();
        }
    }

    public void SumarAcierto()
    {
        aciertos++;
        aciertosText.text = "Aciertos:\n" + aciertos + "/" + totalPiezas;
        RevisarCondicionesFinDeJuego();
    }

    public void SumarFallo()
    {
        fallos++;
        fallosText.text = "Fallos:\n" + fallos + "/" + maxFallos;
        RevisarCondicionesFinDeJuego();
    }

    private void RevisarCondicionesFinDeJuego()
    {
        if (tiempoRestante <= 0 || fallos >= maxFallos || aciertos >= totalPiezas)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}