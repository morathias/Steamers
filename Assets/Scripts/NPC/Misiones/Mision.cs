using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//==========================================
public class Mision : MonoBehaviour {
    protected string informacion;
    public Text mensajeMisionCumplida;
    private Sprite papel;

    protected bool _activa = false;
    //--------------------------------------
    protected virtual void Update() {
        if (_activa) {
            if (condicionCumplida()){
                mensajeMisionCumplida.enabled = true;
                Destroy(gameObject);
            }
        }
    }
    //--------------------------------------
    public void empezarMision() {
        _activa = true;
    }
    //--------------------------------------
    public bool misionTerminada() {
        _activa = false;
        return true;
    }
    //--------------------------------------
    protected virtual bool condicionCumplida() {
        return false;
    }
    //--------------------------------------
    public string getInformacion() {
        return informacion;
    }
}
//==========================================
