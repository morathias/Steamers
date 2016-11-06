using UnityEngine;
using System.Collections;

public class CazarEnemigos : Mision {
    public CQCMob tipoCqcMob;
    public Infantry tipoShotty;
    public Infantry tipoRifleman;
    public Shield tipoArmored;
    public protoScriptAC tipoCaptain;

    public int cantidadACazar = 10;
    int _cantidadCazada;
    int _tipoRandom;

    string _tipoEnemigoACazar;

	void Start () {
        _cantidadCazada = 0;
        _tipoEnemigoACazar = tipoArmored.gameObject.tag;
	}

    protected override bool condicionCumplida()
    {
        if (_cantidadCazada == cantidadACazar)
            return true;
        else return false;
    }
}
