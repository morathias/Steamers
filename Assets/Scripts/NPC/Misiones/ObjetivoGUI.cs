using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetivoGUI : MonoBehaviour {
    bool _mostrando = false;

    public void setMostrando(bool mostrando) {
        _mostrando = mostrando;
    }
    public bool getMostrando() {
        return _mostrando;
    }
}
