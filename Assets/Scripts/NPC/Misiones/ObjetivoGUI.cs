using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetivoGUI : MonoBehaviour {
    private string _infoGral;
    bool _mostrando = false;

    public string infoGral{
        get{ return _infoGral;}

        set{ _infoGral = value;}
    }

    public void setMostrando(bool mostrando) {
        _mostrando = mostrando;
    }
    public bool getMostrando() {
        return _mostrando;
    }
}
