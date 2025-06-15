using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Changer : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadLabScene()
    {
        SceneManager.LoadScene("Laboratorio");
    }

    public void LoadAssemblyScene()
    {
        SceneManager.LoadScene("Clase");
    }

    public void LoadPracticeScene()
    {
        SceneManager.LoadScene("Practice");
    }
}

