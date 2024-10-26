using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarObjetos : MonoBehaviour
{
    string tagObjetivo = "TarjetaGrafica";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagObjetivo))
        {
            Debug.Log("Correcto");
        }else
        {
            Debug.Log("Incorrecto");
        }
    }
}
