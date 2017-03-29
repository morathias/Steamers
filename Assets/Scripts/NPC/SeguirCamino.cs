using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirCamino : MonoBehaviour {
    Transform[] _puntos;
    int puntoActual = 0;

    bool _alcanzoDestino = false;
	void Update () {
        if (puntoActual >= _puntos.Length){
            _alcanzoDestino = true;
            puntoActual = _puntos.Length - 1;
            Debug.Log("punto actual: "+puntoActual+" length: "+_puntos.Length);
        }

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _puntos[puntoActual].position, Time.deltaTime * 3);

        float distancia = Vector3.Distance(gameObject.transform.position, _puntos[puntoActual].position);
        if (distancia <= 0.1f)
            puntoActual++; 
	}

    public void setPuntosCamino(Transform[] puntos) { _puntos = puntos; }

    public bool alcanzoDestino() { return _alcanzoDestino; }
}
