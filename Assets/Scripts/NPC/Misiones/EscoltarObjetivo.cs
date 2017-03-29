using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscoltarObjetivo : Objetivo {
    public GameObject camino;
    Transform[] puntos;

    public Npc npcAEscoltar;
    SeguirCamino _caminoASeguir;
	
	void Start () {
        puntos = new Transform[camino.transform.childCount];

        for (int i = 0; i < camino.transform.childCount; i++)
            puntos[i] = camino.transform.GetChild(i);

        _caminoASeguir = npcAEscoltar.GetComponent<SeguirCamino>();
        _caminoASeguir.setPuntosCamino(puntos);
	}

    void Update() {
        if (_activo)
            _caminoASeguir.enabled = true;
    }

    public override bool condicionCumplida(){
        if (_caminoASeguir.alcanzoDestino()){
            _caminoASeguir.enabled = false;
            _activo = false;
            _terminado = true;
            return true;
        }

        return false;
    }
}
