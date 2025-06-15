using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using TMPro;
using FMOD.Studio;

public class GameManagerClase : MonoBehaviour
{
    EventInstance fondoClase;
    public Slider sliderVolumen;
    public TextMeshPro valorVolumen;

    public Timer_Manager timer; 
    public int totalCorrectPieces = 0;
    public ScoreManager scoreManager;
    public GameObject scoreScreen;
    public GameObject puertaTorrePos;

    private void Start()
    {
        fondoClase = RuntimeManager.CreateInstance("event:/ModoLibreFondo");

        float volumenGuardado = PlayerPrefs.GetFloat("volumen_musica", 100f);
        sliderVolumen.value = volumenGuardado;
        fondoClase.setVolume(Mathf.Clamp01(volumenGuardado / 100f));
        ActualizarTexto(volumenGuardado);
        sliderVolumen.onValueChanged.AddListener(ActualizarVolumen);
        if (puertaTorrePos != null)
        {
            puertaTorrePos.SetActive(false); // Imagen1: desactivar al inicio
        }

        fondoClase.start();
    }

    private void Update()
    {
        fondoClase.getPlaybackState(out PLAYBACK_STATE estado);
        if (estado == PLAYBACK_STATE.STOPPED)
        {
            fondoClase.start();
        }
    }

    private void OnDestroy()
    {
        fondoClase.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fondoClase.release();
    }

    private void ActualizarVolumen(float valor)
    {
        PlayerPrefs.SetFloat("volumen_musica", valor);
        PlayerPrefs.Save();

        fondoClase.setVolume(Mathf.Clamp01(valor / 100f));
        ActualizarTexto(valor);
    }

    private void ActualizarTexto(float valor)
    {
        valorVolumen.text = Mathf.RoundToInt(valor).ToString() + "%";
    }

    public void RegisterCorrectPiece()
    {
        totalCorrectPieces++;

        if (totalCorrectPieces >= 8)
        {
            if (puertaTorrePos != null)
            {
                puertaTorrePos.SetActive(true); 
                Debug.Log("PuertaTorre_pos activada.");
            }
            timer.StopTimer();
            scoreManager.StopScore();
            scoreScreen.SetActive(true);
    Debug.Log("¡Se colocaron las 9 piezas correctamente! Timer detenido.");
        }
    }

}
