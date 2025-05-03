using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using FMODUnity;
using FMOD.Studio;

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
    private float tiempoRestante = 422f;
    private bool contandoTiempo = true;

    public static GameManager instance;

    EventInstance fondo;
    EventInstance correcto;
    EventInstance incorrecto;

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

        fondo = RuntimeManager.CreateInstance("event:/7MinutosFondo");
        correcto = RuntimeManager.CreateInstance("event:/Correcto");
        incorrecto = RuntimeManager.CreateInstance("event:/Incorrecto");
        fondo.start();
    }

    private void Update()
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60f);
            tiempoText.text = $"Tiempo restante:\n{minutos:D2}:{segundos:D2}";
        }
        else
        {
            tiempoRestante = 0;
            contandoTiempo = false;
            tiempoText.text = "Se acabó el tiempo";
        }

        fondo.getPlaybackState(out PLAYBACK_STATE estado);
        if (estado == PLAYBACK_STATE.STOPPED)
        {
            RevisarCondicionesFinDeJuego();
        }
    }

    public void SumarAcierto()
    {
        correcto.start();
        aciertos++;
        aciertosText.text = "Aciertos:\n" + aciertos + "/" + totalPiezas;
        RevisarCondicionesFinDeJuego();
    }

    public void SumarFallo()
    {
        incorrecto.start();
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

    private void OnDestroy()
    {
        fondo.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fondo.release();
        correcto.release();
        incorrecto.release();
    }
}