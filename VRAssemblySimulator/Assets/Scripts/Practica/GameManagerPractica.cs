using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class GameManagerPractica : MonoBehaviour
{
    public TextMeshPro tiempoText;
    public TextMeshPro aciertosText;
    public TextMeshPro fallosText;

    [SerializeField] private GameObject[] piezasParaAcertar;

    private int aciertos = 0;
    private int totalPiezas;
    private int fallos = 0;
    private int maxFallos = 12;
    private float tiempoRestante = 1200;

    public Slider sliderVolumen;
    public TextMeshPro valorVolumen;

    public static GameManagerPractica instance;

    EventInstance fondo;
    EventInstance correcto;
    EventInstance incorrecto;

    public Timer_Manager timer;
    public int totalCorrectPieces = 0;
    public ScoreManager scoreManager;
    public GameObject scoreScreen;

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

        fondo = RuntimeManager.CreateInstance("event:/ModoLibreFondo");
        correcto = RuntimeManager.CreateInstance("event:/Correcto");
        incorrecto = RuntimeManager.CreateInstance("event:/Incorrecto");

        float volumenGuardado = PlayerPrefs.GetFloat("volumen_musica", 100f);
        sliderVolumen.value = volumenGuardado;
        fondo.setVolume(Mathf.Clamp01(volumenGuardado / 100f));
        ActualizarTexto(volumenGuardado);
        sliderVolumen.onValueChanged.AddListener(ActualizarVolumen);

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

    private void ActualizarVolumen(float valor)
    {
        PlayerPrefs.SetFloat("volumen_musica", valor);
        PlayerPrefs.Save();

        fondo.setVolume(Mathf.Clamp01(valor / 100f));
        ActualizarTexto(valor);
    }

    private void ActualizarTexto(float valor)
    {
        valorVolumen.text = Mathf.RoundToInt(valor).ToString() + "%";
    }

    public void RegisterCorrectPiece()
    {
        totalCorrectPieces++;

        if (totalCorrectPieces >= 9)
        {
            timer.StopTimer();
            scoreManager.StopScore();
            scoreScreen.SetActive(true);
            Debug.Log("¡Se colocaron las 9 piezas correctamente! Timer detenido.");
        }
    }
}
