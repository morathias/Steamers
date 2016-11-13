using UnityEngine;
using System.Collections;
//=============================================================================
public class CazarEnemigos : Mision {
    public Overlord[] tiposEnemigos;

    public int cantidadACazar = 10;
    int _cantidadCazada;
    int _tipoRandom;

    string _tipoEnemigoACazar;
    //-------------------------------------------------------------------------
	void Start () {
        _cantidadCazada = 0;
        _tipoRandom = Random.Range(0, tiposEnemigos.Length - 1);
        _tipoEnemigoACazar = tiposEnemigos[_tipoRandom].tag;

        informacion = "Kill " + cantidadACazar + " enemies of type " + _tipoEnemigoACazar;
	}
    //-------------------------------------------------------------------------
    protected override bool condicionCumplida()
    {
        if (_cantidadCazada == cantidadACazar)
            return true;
        else return false;
    }
    //-------------------------------------------------------------------------
    public void seMatoEnemigo(GameObject enemigo) {
        if (_activa){
            if (enemigo.tag == _tipoEnemigoACazar)
            {
                _cantidadCazada++;
                Debug.Log(_cantidadCazada);
            }
        }
    }
}
//=============================================================================
