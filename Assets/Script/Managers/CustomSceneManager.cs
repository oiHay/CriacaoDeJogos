 using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public static void LoadNextScene()
    {
        SceneTransition.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void Reset()
    {
        SceneTransition.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void MainMenu()
    {
        SceneTransition.LoadScene(0);
    }
}
