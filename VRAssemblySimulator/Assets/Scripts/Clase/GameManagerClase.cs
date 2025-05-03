using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class GameManagerClase : MonoBehaviour
{
    EventInstance fondoClase;

    private void Start()
    {
        fondoClase = RuntimeManager.CreateInstance("event:/ModoLibreFondo");
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
}
