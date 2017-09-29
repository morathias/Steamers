using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EsceneManager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void cambiarEscena(int index)
    {
        //LoadingScreenManager.LoadScene(index);
        SceneManager.LoadScene(index);
        Time.timeScale = 1;
    }

    public void CerrarJuego()
    {
        Application.Quit();
    }
}
