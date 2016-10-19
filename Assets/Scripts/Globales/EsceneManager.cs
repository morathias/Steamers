using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EsceneManager : MonoBehaviour
{
    public void cambiarEscena(int index)
    {
        SceneManager.LoadScene(index);
    }
}
