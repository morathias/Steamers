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

    private Vector3 _npcPos;

    public Mision() {
        
    }

    public void setUp()
    {
        if (enEspera)
        {
            MisionesManager.getInstance().agregarMisionInactiva(this);
        }

        if (isAlreadyActivated)
            empezarMision();
    }
    //--------------------------------------
    protected virtual void Update() {
        if (_activa) {
            if (objetivosCumplidos()){
                Debug.Log("Mision Cumplida");
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

        MisionesManager.getInstance().removerMisionInactiva(this);

        //chequeo para misiones que estan activas en el mundo pero el jugador las desconoce
        //asi no aparecenen en el journal menu
        if (!isAlreadyActivated)
        {
            Debug.Log("agregando al journal");
            MisionesManager.getInstance().agregarMisionEnCurso(this);
        }
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
        MisionesManager.getInstance().removerMisionEnCurso(this);
        MisionesManager.getInstance().agregarMisionTerminada(this);

        for (int i = 0; i < recompensas.Count; i++){
            int randomPos = Random.Range(-3, 3);
            Vector3 pos = new Vector3(_npcPos.x + randomPos, _npcPos.y, _npcPos.z + randomPos);

            GameObject.Instantiate(recompensas[i].gameObject, pos, Quaternion.identity);
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
