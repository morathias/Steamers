using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour {
    public Image overlayNegro;

    public float velocidad;

    void Awake() {
        fadeIn();
    }

    void fadeIn() {
        overlayNegro.CrossFadeAlpha(0.0f, velocidad * Time.deltaTime, false);
    }
    public void fadeOut() {
        overlayNegro.canvasRenderer.SetAlpha(0.0f);
        overlayNegro.CrossFadeAlpha(1.0f, velocidad * Time.deltaTime, false);
    }
}
