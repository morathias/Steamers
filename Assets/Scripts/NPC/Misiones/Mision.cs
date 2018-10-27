using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//==========================================
[System.Serializable]
public class Mision {
    public string nombre;
    [TextArea]
    public string informacionGeneral;

    public int valorEtico = 0;

    public bool enEspera = false;
    protected bool _activa = false;
    public bool isAlreadyActivated = false;

    public List<Item> recompensas;

    [SerializeField]
    public List<Objetivo> objetivos;

    Objetivo _objetivoActivo;
    int _objetivoActivoIndex = 0;

    private Vector3 _npcPos;

    private Text _misionCumplidaTxt;

    public Mision() {}
    //--------------------------------------
    public void setUp()
    {
        _misionCumplidaTxt = GameObject.Find("misionCumplida").GetComponent<Text>();
        if (enEspera)
        {
            MisionesManager.getInstance().agregarMisionInactiva(this);
        }

        if (isAlreadyActivated)
            empezarMision();
    }
    //--------------------------------------
    public virtual void Update() {
        if (_activa) {
            if (objetivosCumplidos()){
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
        objetivos[0].gameObject.SetActive(true);

        MisionesManager.getInstance().removerMisionInactiva(this);

        //chequeo para misiones que estan activas en el mundo pero el jugador las desconoce
        //asi no aparecenen en el journal menu
        if (!isAlreadyActivated)
        {
            MisionesManager.getInstance().agregarMisionEnCurso(this);
        }
    }
    //--------------------------------------
    public void siguienteObjetivo() {
        _objetivoActivoIndex++;

        if (_objetivoActivoIndex < objetivos.Count){
            _objetivoActivo = objetivos[_objetivoActivoIndex];
            _objetivoActivo.activo = true;
        }
    }
    //--------------------------------------
    public bool misionTerminada() {
        _objetivoActivoIndex = 0;
        _activa = false;
        MisionesManager.getInstance().removerMisionEnCurso(this);
        MisionesManager.getInstance().agregarMisionTerminada(this);

        for (int i = 0; i < recompensas.Count; i++){
            int randomPos = Random.Range(-3, 3);
            Vector3 pos = new Vector3(_npcPos.x + randomPos, _npcPos.y, _npcPos.z + randomPos);

            GameObject.Instantiate(recompensas[i].gameObject, pos, Quaternion.identity);
        }

        if (_misionCumplidaTxt)
        {
            _misionCumplidaTxt.enabled = true;
        }

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
