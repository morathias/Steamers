using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//============================================================
[System.Serializable]
public class EncontrarItem : Objetivo {
    [HideInInspector]
    public Item.Tipo itemAEncontrar;

    [HideInInspector]
    public int cantidad;
    static int _cantidadAgarrada = 0;

    [HideInInspector]
    public bool esRandom = false;
    //-----------------------------------------------------
    public delegate void itemAgarradoHandler(Item item);
    public static event itemAgarradoHandler itemAgarrado;

    void OnEnable() { itemAgarrado += seAgarroItem; }

    public static void onItemAgarrado(Item item) {
        if (itemAgarrado != null)
        {
            itemAgarrado(item);
        }
    }
    //-----------------------------------------------------
    void seAgarroItem(Item item) {
        if (_activo){
            if (item.tipo == itemAEncontrar)
            {
                _cantidadAgarrada++;
            }
        }
    }
    //-----------------------------------------------------
    public override bool condicionCumplida(){
        if (_cantidadAgarrada == cantidad){
            _terminado = true;
            _activo = false;
            itemAgarrado -= seAgarroItem;
            return true;
        }
        return false;
    }
    //-----------------------------------------------------
    public override object downCast()
    {
        return this;
    }
}
//============================================================
