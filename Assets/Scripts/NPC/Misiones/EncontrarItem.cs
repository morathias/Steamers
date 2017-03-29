using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//============================================================
public class EncontrarItem : Objetivo {
    public Item.Tipo itemAEncontrar;

    public int cantidad;
    static int _cantidadAgarrada = 0;

    public bool esRandom = false;
    //-----------------------------------------------------
    public delegate void itemAgarradoHandler(Item item);
    public static event itemAgarradoHandler itemAgarrado;

    void OnEnable() { itemAgarrado += seAgarroItem; }

    public static void onItemAgarrado(Item item) {
        if (itemAgarrado != null)
        {
            itemAgarrado(item);
            Debug.Log("evento ejecutado");
        }
    }
    //-----------------------------------------------------
    void seAgarroItem(Item item) {
        if (_activo){
            if (item.tipo == itemAEncontrar)
            {
                _cantidadAgarrada++;
                Debug.Log("se agarro item " + _cantidadAgarrada);
            }
        }
    }
    //-----------------------------------------------------
    public override bool condicionCumplida(){
        if (_cantidadAgarrada == cantidad){
            Debug.Log("se consiguieron todos los items " + _cantidadAgarrada);
            _terminado = true;
            _activo = false;
            itemAgarrado -= seAgarroItem;
            return true;
        }
        return false;
    }
    //-----------------------------------------------------
}
//============================================================
