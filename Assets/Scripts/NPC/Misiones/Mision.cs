using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//==========================================
public class Mision : MonoBehaviour {
    public Text informacion;
    public Text mensajeMisionCumplida;
    public Image papel;

    bool _activa = false;
    //--------------------------------------
    protected virtual void Update() {
        if (_activa) {
            if (condicionCumplida())
                mensajeMisionCumplida.enabled = true;
        }
    }
    //--------------------------------------
    public void empezarMision() {
        _activa = true;
    }
    //--------------------------------------
    public void terminarMision() {
        _activa = false;
    }
    //--------------------------------------
    protected virtual bool condicionCumplida() {
        return false;
    }
    //--------------------------------------
}
//==========================================
