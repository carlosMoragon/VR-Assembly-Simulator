using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class GameManagerMenu : MonoBehaviour
{
    EventInstance fondoMenu;

    private void Start()
    {
        fondoMenu = RuntimeManager.CreateInstance("event:/MenuFondo");
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
}
