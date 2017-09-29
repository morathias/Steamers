using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSonido : MonoBehaviour {
    public Slider sliderVolumenMusica;

    void Start() {
        sliderVolumenMusica.value = Variables.volumenMusica;
    }

	public void cambiarVolumen(){
        Variables.volumenMusica = sliderVolumenMusica.value;
    }
}
