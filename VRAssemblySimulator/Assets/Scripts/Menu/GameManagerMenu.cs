using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using TMPro;
using FMOD.Studio;

public class GameManagerMenu : MonoBehaviour
{
    EventInstance fondoMenu;

    public Slider sliderVolumen;
    public TextMeshPro valorVolumen;

    private void Start()
    {
        fondoMenu = RuntimeManager.CreateInstance("event:/MenuFondo");

        float volumenGuardado = PlayerPrefs.GetFloat("volumen_musica", 100f);
        sliderVolumen.value = volumenGuardado;
        fondoMenu.setVolume(Mathf.Clamp01(volumenGuardado / 100f));
        ActualizarTexto(volumenGuardado);
        sliderVolumen.onValueChanged.AddListener(ActualizarVolumen);

        fondoMenu.start();
    }

    private void Update()
    {
        fondoMenu.getPlaybackState(out PLAYBACK_STATE estado);
        if (estado == PLAYBACK_STATE.STOPPED)
        {
            fondoMenu.start();
        }
    }

    private void OnDestroy()
    {
        fondoMenu.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fondoMenu.release();
    }

    private void ActualizarVolumen(float valor)
    {
        PlayerPrefs.SetFloat("volumen_musica", valor);
        PlayerPrefs.Save();

        fondoMenu.setVolume(Mathf.Clamp01(valor / 100f));
        ActualizarTexto(valor);
    }

    private void ActualizarTexto(float valor)
    {
        valorVolumen.text = Mathf.RoundToInt(valor).ToString() + "%";
    }
}
