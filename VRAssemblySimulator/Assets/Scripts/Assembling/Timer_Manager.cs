using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer_Manager : MonoBehaviour
{
    [SerializeField] TextMeshPro timer_txt;
    float time;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);
        timer_txt.text = string.Format("{0:00}:{1:00}", minutes, sec);
    }
}
