using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CazarBoss : Objetivo {

    public GameObject boss;
    //-------------------------------------------------------------------------
    public override bool condicionCumplida()
    {
        if (boss == null)
        {
            _terminado = true;
            _activo = false;
            return true;
        }

        else return false;
    }
    //-------------------------------------------------------------------------
    public override object downCast()
    {
        return this;
    }
}
