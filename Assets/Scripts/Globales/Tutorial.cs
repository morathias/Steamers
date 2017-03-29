using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {
    bool _terminado = false;
    int _contadorBalas = 14;

    public Image dialogoBox;
    public Text mensajeTxt;
    public Canvas canvasTuto;

    void OnTriggerEnter(Collider trigger) {
        dialogoBox.enabled = true;
        mensajeTxt.enabled = true;
    }

    void Update() {
        if (Input.GetButtonDown("Fire1"))
            _contadorBalas--;

        if (_contadorBalas == 0){
            canvasTuto.enabled = true;
            Time.timeScale = 0;
        }

        if (canvasTuto.enabled){
            if (Input.GetKeyDown(KeyCode.R)){
                Time.timeScale = 1;
                Destroy(canvasTuto);
            }
        }
    }

    void OnTriggerExit(Collider trigger) {
        Destroy(gameObject);
    }
}
