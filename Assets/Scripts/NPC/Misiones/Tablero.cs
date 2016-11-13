using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
//=================================================================================================
public class Tablero : MonoBehaviour {
    public Mision[] misiones;
    List<Mision> _misionesActivas;

    public Text accion;
    public Text informacion;
    public Text misionAceptadaTxt;
    public Image papel;
    public Image fondo;
    public Image[] sliders;
    public GameObject botonAceptar;

    Prota _prota;
    int _misionIndex = 0;

    MenuPausa _menuPausa;

    enum estados {
        esperando,
        mostrando
    }
    estados _estado;
    //---------------------------------------------------------------------------------------------
	void Start () {
        _prota = GameObject.Find("Prota").GetComponent<Prota>();
        _menuPausa = GameObject.Find("gameMaster").GetComponent<MenuPausa>();
        _misionesActivas = new List<Mision>();
	}
    //---------------------------------------------------------------------------------------------
	void Update () {
        switch (_estado) {
            case estados.esperando:
                if (Vector3.Distance(gameObject.transform.position, _prota.transform.position) < 3)
                {
                    accion.enabled = true;
                    accion.text = "E to examine Board";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        _estado = estados.mostrando;
                        _prota.estaHablando(true);
                        _menuPausa.enabled = false;
                        fondo.enabled = true;
                        for (int i = 0; i < sliders.Length; i++)
                            sliders[i].enabled = true;
                        botonAceptar.SetActive(true);
                    }
                }
                else
                    accion.enabled = false;
                break;

            case estados.mostrando:
                accion.text = "Esc to close";
                mostrarMision(misiones[_misionIndex].getInformacion(), papel);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _estado = estados.esperando;
                    _prota.terminoDeHablar();
                    informacion.enabled = false;
                    papel.enabled = false;
                    _menuPausa.enabled = true;
                    fondo.enabled = false;
                    for (int i = 0; i < sliders.Length; i++)
                        sliders[i].enabled = false;
                    botonAceptar.SetActive(false);
                    misionAceptadaTxt.enabled = false;
                }
                break;
        }
	}
    //---------------------------------------------------------------------------------------------
    void mostrarMision(string texto, Image papel) {
        informacion.enabled = true;
        informacion.text = texto;
        papel.enabled = true;
    }
    //---------------------------------------------------------------------------------------------

    public List<Mision> getMisionDeTipo(string tipo) {
        List<Mision> misionesDeTipo = new List<Mision>();
        {
            for (int i = 0; i < _misionesActivas.Count; i++)
            {
                if (_misionesActivas[i]){
                    if (_misionesActivas[i].gameObject.tag == tipo)
                        misionesDeTipo.Add(_misionesActivas[i]);
                }
            }
        }

        return misionesDeTipo;
    }

    public void activarMision() {
        misiones[_misionIndex].empezarMision();
        _misionesActivas.Add(misiones[_misionIndex]);
        misionAceptadaTxt.enabled = true;
    }
}
//=================================================================================================
