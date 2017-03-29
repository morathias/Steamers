using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetivo : MonoBehaviour {
    public string informacion;

    protected bool _activo = false;
    protected bool _terminado = false;
    
    public virtual bool condicionCumplida(){ return false;}

    public bool terminado{
        get { return _terminado; }
    }

    public bool activo{
        get { return _activo; }
        set { _activo = value; }
    }
}
