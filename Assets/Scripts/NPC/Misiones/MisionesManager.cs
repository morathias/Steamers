using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//======================================================================
public class MisionesManager {
    private List<Mision> _misionesTerminadas;
    private List<Mision> _misionesEnEspera;
    private List<Mision> _misionesEnCurso;

    private static MisionesManager _instance;
    //------------------------------------------------------------------
    private MisionesManager() {
        Debug.Log("MisionesManager inicializado");

        _misionesEnEspera = new List<Mision>();
        _misionesEnCurso = new List<Mision>();
        _misionesTerminadas = new List<Mision>();
    }
    //------------------------------------------------------------------
    public static MisionesManager getInstance() {
        if (_instance == null) {
            _instance = new MisionesManager();
        }
        return _instance;
    }
    //------------------------------------------------------------------
    public void agregarMisionTerminada(Mision mision) {
        _misionesTerminadas.Add(mision);
    }
    //------------------------------------------------------------------
    public void agregarMisionEnCurso(Mision mision){
        _misionesEnCurso.Add(mision);
    }
    //------------------------------------------------------------------
    public void agregarMisionInactiva(Mision mision) {
        Debug.Log("agregando mision inactiva");
        _misionesEnEspera.Add(mision);
    }
    //------------------------------------------------------------------
    public void removerMisionTerminada(Mision mision){
        _misionesTerminadas.Remove(mision);
    }
    //------------------------------------------------------------------
    public void removerMisionEnCurso(Mision mision){
        _misionesEnCurso.Remove(mision);
    }
    //------------------------------------------------------------------
    public void removerMisionInactiva(Mision mision){
        Debug.Log("removing mision");
        _misionesEnEspera.Remove(mision);
    }
    //------------------------------------------------------------------
    public List<Mision> getMisionesEnEspera() {
        return _misionesEnEspera;
    }
    //------------------------------------------------------------------
    public List<Mision> getMisionesEnCurso(){
        return _misionesEnCurso;
    }
    //------------------------------------------------------------------
    public List<Mision> getMisionesTerminadas(){
        return _misionesTerminadas;
    }
    //------------------------------------------------------------------
}
//======================================================================
