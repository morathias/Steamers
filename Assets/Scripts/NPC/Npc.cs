using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//===================================================================================================
public class Npc : MonoBehaviour {
    public Image dialogoBox;
    public Text mensaje;
    public Mision[] misiones;
    public Text accion;

    GameObject _prota;
    Dialogo _dialogoBox;

    public int inicioDialogo = 0;
    public int finDialogo = 0;
    

    enum estados {
        esperando,
        hablando,
    }
    estados _estado;
    //-----------------------------------------------------------------------------------------------
	void Start () {
        _estado = estados.esperando;
        _prota = GameObject.Find("Prota");
        _dialogoBox = GetComponentInChildren<Dialogo>();

        _dialogoBox.setInicioDialogo(inicioDialogo);
        _dialogoBox.setFinDialogo(finDialogo);
	}
    //-----------------------------------------------------------------------------------------------
	void Update () {
        switch (_estado) {
            case estados.esperando:
                if (Vector3.Distance(gameObject.transform.position, _prota.transform.position) < 3)
                {
                    accion.enabled = true;
                    accion.text = "E to talk";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        _dialogoBox.setInicioDialogo(inicioDialogo);
                        _prota.GetComponent<Prota>().estaHablando(true);
                        dialogoBox.enabled = true;
                        mensaje.enabled = true;
                        _estado = estados.hablando;
                    }
                }
                else
                {
                    if(accion.enabled)
                    accion.enabled = false;
                }
            break;
                
            case estados.hablando:
            accion.text = "E: next";

            if (_dialogoBox.finDialogo())
            {
                Debug.Log("findialogo");
                _estado = estados.esperando;
                _prota.GetComponent<Prota>().terminoDeHablar();
            }
            break;
        }
	}
    //-----------------------------------------------------------------------------------------------
    public bool estaHablando() {
        if (_estado == estados.hablando)
            return true;
        else
            return false;
    }
}
//===================================================================================================
