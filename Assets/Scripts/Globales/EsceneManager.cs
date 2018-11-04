using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EsceneManager : MonoBehaviour
{
    private Image _fadeOverlay;
    private int _levelToLoad;

    public float fadeTime = 1f;

    void Start() {
        _fadeOverlay = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        StartCoroutine(startFadingOut());
    }

    public void cambiarEscena(int index)
    {
        _levelToLoad = index;
        StartCoroutine(startFadingIn());
    }

    IEnumerator startFadingOut() {
        Color color = _fadeOverlay.color;

        float timeStarted = Time.realtimeSinceStartup;

        while (color.a > 0f){
            float timeSinceStarted = Time.realtimeSinceStartup - timeStarted;
            float percentage = timeSinceStarted / fadeTime;

            color.a = Mathf.Lerp(1f, 0f, percentage);
            _fadeOverlay.color = color;
            yield return null;
        }

        _fadeOverlay.enabled = false;
    }

    IEnumerator startFadingIn()
    {
        _fadeOverlay.enabled = true;
        Color color = _fadeOverlay.color;

        float timeStarted = Time.realtimeSinceStartup;

        while (color.a < 1f)
        {
            float timeSinceStarted = Time.realtimeSinceStartup - timeStarted;
            float percentage = timeSinceStarted / fadeTime;

            color.a = Mathf.Lerp(0f, 1f, percentage);
            _fadeOverlay.color = color;
            yield return null;
        }

        onFadeCompleted();
    }

    private void onFadeCompleted() {
        SceneManager.LoadScene(_levelToLoad);
        Time.timeScale = 1;
    }

    public void CerrarJuego()
    {
        Application.Quit();
    }
}
