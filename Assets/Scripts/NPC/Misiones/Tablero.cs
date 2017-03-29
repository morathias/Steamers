using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
//=================================================================================================
public class Tablero : MonoBehaviour {
    public List<Mision> misiones;

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
        DontDestroyOnLoad(gameObject);
        _prota = GameObject.Find("Prota").GetComponent<Prota>();
        _menuPausa = GameObject.Find("gameMaster").GetComponent<MenuPausa>();
	}
    //---------------------------------------------------------------------------------------------
	void Update () {

        if(_prota == null)
            _prota = GameObject.Find("Prota").GetComponent<Prota>();
        if (accion == null)
            accion = GameObject.Find("Accion").GetComponent<Text>();

        switch (_estado) {
            case estados.esperando:
                if (Vector3.Distance(gameObject.transform.position, _prota.transform.position) < 3){
                    accion.enabled = true;
                    accion.text = "E to examine Board";

                    if (Input.GetKeyDown(KeyCode.E))
                        abrirTablero();
                }
                else
                    accion.enabled = false;
                break;

            case estados.mostrando:
                accion.text = "Esc to close";

                navegarTablero();

                mostrarMision(misiones[_misionIndex].getInformacion(), papel);

                if (Input.GetKeyDown(KeyCode.Escape))
                    cerrarTablero();
                break;
        }
	}
    //---------------------------------------------------------------------------------------------
    void mostrarMision(string texto, Image papel) {
        informacion.enabled = true;
        informacion.text = texto;
        papel.enabled = true;

        if (misiones[_misionIndex].getActiva())
            misionAceptadaTxt.enabled = true;
        else
            misionAceptadaTxt.enabled = false;
    }
    //---------------------------------------------------------------------------------------------
    public void activarMision() {
        misiones[_misionIndex].empezarMision();
    }

    void navegarTablero() {
        if (Input.GetKeyDown(KeyCode.D))
            _misionIndex++;
        if (Input.GetKeyDown(KeyCode.A))
            _misionIndex--;
        if (_misionIndex == misiones.Count)
            _misionIndex = misiones.Count - 1;
    }

    void cerrarTablero() {
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

    void abrirTablero() {
        _estado = estados.mostrando;
        _prota.estaHablando(true);
        _menuPausa.enabled = false;
        fondo.enabled = true;
        for (int i = 0; i < sliders.Length; i++)
            sliders[i].enabled = true;
        botonAceptar.SetActive(true);
    }
}
//=================================================================================================
