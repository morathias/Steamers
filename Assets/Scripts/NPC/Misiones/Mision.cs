using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//==========================================
public class Mision : MonoBehaviour {
    [TextArea]
    public string informacionGeneral;

    public Text informacionGeneralTxt;
    public string nombre;
    public Text nombreTxt;
    private Image tick;

    public Text mensajeMisionCumplida;
    private Sprite papel;

    public int valorEtico = 0;

    public Button misionBtn;
    protected bool _activa = false;
    public bool isAlreadyActivated = false;

    public Item[] recompensas;
    public List<Objetivo> objetivos;
    public Text[] objetivosTxt;
    Objetivo _objetivoActivo;

    void Start() {
        if (isAlreadyActivated) empezarMision();
        tick = misionBtn.gameObject.transform.Find("Tick").GetComponent<Image>();
    }
    //--------------------------------------
    protected virtual void Update() {
        if (mensajeMisionCumplida == null)
            mensajeMisionCumplida = GameObject.Find("misionCumplida").GetComponent<Text>();

        if (_activa) {
            if (objetivosCumplidos()){
                Debug.Log("Mision Cumplida");
                mensajeMisionCumplida.enabled = true;
                misionTerminada();
            }

            if (_objetivoActivo.condicionCumplida())
                siguienteObjetivo();
        }
    }
    //--------------------------------------
    public void empezarMision() {
        _activa = true;
        _objetivoActivo = objetivos[0];
        _objetivoActivo.activo = true;

        for (int i = 0; i < objetivos.Count; i++)
            objetivosTxt[i].text = objetivos[i].informacion;
        nombreTxt.text = nombre;
        informacionGeneralTxt.text = informacionGeneral;
        misionBtn.gameObject.SetActive(true);
    }
    //--------------------------------------
    public void siguienteObjetivo() {
        objetivos.Remove(_objetivoActivo);
        objetivos.Add(_objetivoActivo);

        _objetivoActivo = objetivos[0];
        _objetivoActivo.activo = true;
    }
    //--------------------------------------
    public bool misionTerminada() {
        _activa = false;

        for (int i = 0; i < recompensas.Length; i++){
            int randomPos = Random.Range(-3, 3);
            Vector3 pos = new Vector3(transform.position.x + randomPos, transform.position.y, transform.position.z + randomPos);

            Instantiate(recompensas[i].gameObject, pos, transform.rotation);
        }
        tick.gameObject.SetActive(true);
        gameObject.SetActive(_activa);
        return true;
    }
    //--------------------------------------
    bool objetivosCumplidos() {
        for (int i = 0; i < objetivos.Count; i++){
            if (!objetivos[i].terminado)
                return false;
        }
        return true;
    }
    //--------------------------------------
    public string getInformacion() {
        return informacionGeneral;
    }

    public bool getActiva() {
        return _activa;
    }
}
//==========================================
