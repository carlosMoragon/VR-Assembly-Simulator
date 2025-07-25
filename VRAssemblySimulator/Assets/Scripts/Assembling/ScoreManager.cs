using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public float score = 100f;
    public TextMeshPro scoreText; 
    public float timePenaltyPerSecond = 100f / 300f; 
    public float errorPenalty = 5f; 
    private bool isRunning = true;

    private void Start()
    {
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isRunning)
        {
            score -= timePenaltyPerSecond * Time.deltaTime;
            score = Mathf.Max(score, 0); // No bajar de 0
        }
    }

    public void StopScore()
    {
        isRunning = false;

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true); 
            UpdateScoreText(); 
        }
    }

    public void PenalizeError()
    {
        score -= errorPenalty;
        score = Mathf.Max(score, 0); 
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntuación: " + Mathf.CeilToInt(score);
        }
    }

    public int GetFinalScore()
    {
        return Mathf.CeilToInt(score);
    }
}
