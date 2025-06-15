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

    public Slider sliderVolumen;
    public TextMeshPro valorVolumen;

    public static GameManagerPractica instance;

    EventInstance fondo;

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

        fondo = RuntimeManager.CreateInstance("event:/ModoLibreFondo");

        float volumenGuardado = PlayerPrefs.GetFloat("volumen_musica", 100f);
        sliderVolumen.value = volumenGuardado;
        fondo.setVolume(Mathf.Clamp01(volumenGuardado / 100f));
        ActualizarTexto(volumenGuardado);
        sliderVolumen.onValueChanged.AddListener(ActualizarVolumen);

        fondo.start();
    }

    private void Update()
    {

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
}
