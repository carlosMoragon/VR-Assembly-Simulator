using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Detect_object : MonoBehaviour
{
    private bool alreadyPlaced = false;
    private GameManagerClase gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManagerClase>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (alreadyPlaced) return;

        if (other.gameObject.tag == this.gameObject.tag)
        {
            other.transform.SetParent(transform);
            other.transform.localPosition = Vector3.zero;

            XRGrabInteractable grab = other.gameObject.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                grab.enabled = false;
            }

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            else
            {
                Debug.LogWarning("No se encontró un Rigidbody en " + other.gameObject.name);
            }

            alreadyPlaced = true;
            if(this.gameObject.tag == "PuertaTorre")
            {
                if(gameManager != null)
                {
                    gameManager.ReturnMenu();
                }
            }
            else if (gameManager != null)
            {
                gameManager.RegisterCorrectPiece();
            }
            else
            {
                Debug.LogError("GameManager no encontrado en la escena.");
            }

        }
        else
        {
            Debug.Log("Los tags NO coinciden. No se realiza ninguna acción.");
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.PenalizeError();
            }
        }
    }
    
}
