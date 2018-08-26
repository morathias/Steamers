using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//=============================================================
public enum TiposObjetivos {
    CazarEnemigos,
    CazarBoss,
    EncontrarItem,
    EscoltarObjetivo,
    HablarBoss,
    HablarNpc,
    IrAZona,
    CantidadTipos
}
//=============================================================
[System.Serializable]
public class Objetivo: MonoBehaviour{
    [HideInInspector]
    public string informacion;
    [HideInInspector]
    public TiposObjetivos tipo;
    
    protected bool _activo = false;
    protected bool _terminado = false;
    //---------------------------------------------------------
    public virtual bool condicionCumplida(){ return false;}
    //---------------------------------------------------------
    public bool terminado{
        get { return _terminado; }
    }
    //---------------------------------------------------------
    public bool activo{
        get { return _activo; }
        set { _activo = value; }
    }
    //---------------------------------------------------------
    public virtual object downCast() {
        return null;
    }
    //---------------------------------------------------------
}
//=============================================================
