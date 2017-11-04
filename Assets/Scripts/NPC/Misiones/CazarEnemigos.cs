using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//=============================================================================
public class CazarEnemigos : Objetivo {
    public List<Overlord> tiposEnemigos;
    GameObject[] enemigos;

    public bool esRandom = false;

    public int cantidadACazar = 10;
    int _cantidadCazada;
    int _tipoRandom;
    string _tipoEnemigoACazar;
    //-------------------------------------------------------------------------
	void Start () {
        _cantidadCazada = 0;

        if (esRandom){
            _tipoRandom = Random.Range(0, tiposEnemigos.Count - 1);
            _tipoEnemigoACazar = tiposEnemigos[_tipoRandom].tag;

            informacion = "Kill " + cantidadACazar + " enemies of type " + _tipoEnemigoACazar;

            enemigos = GameObject.FindGameObjectsWithTag(_tipoEnemigoACazar);
        }
        else
            cantidadACazar = tiposEnemigos.Count;

	}
    //-------------------------------------------------------------------------
    void Update() {
        if (esRandom){
            for (int i = 0; i < enemigos.Length; i++)
                seMatoEnemigo(enemigos[i]);
        }

        else{
            for (int i = 0; i < tiposEnemigos.Count; i++)
                seMatoEnemigo(tiposEnemigos[i]);
        }
    }
    //-------------------------------------------------------------------------
    public override bool condicionCumplida(){
        if (_cantidadCazada == cantidadACazar){
            _terminado = true;
            _activo = false;
            return true;
        }

        else if (tiposEnemigos.Count == 0)
            return true;

        else return false;
    }
    //-------------------------------------------------------------------------
    public void seMatoEnemigo(GameObject enemigo) {
        if (activo){
            if (enemigo == null){
                _cantidadCazada++;
            }
        }
    }
    //-------------------------------------------------------------------------
    public void seMatoEnemigo(Overlord enemigo){
        if (activo){
            if (enemigo == null){
                _cantidadCazada++;
                tiposEnemigos.Remove(enemigo);
                Debug.Log("cantidad cazada" + _cantidadCazada);
            }
        }
    }
}
//=============================================================================
