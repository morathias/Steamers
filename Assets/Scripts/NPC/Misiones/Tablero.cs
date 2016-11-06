using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tablero : MonoBehaviour {
    public Mision[] misiones;
    public Text accion;
    Mision[] _misionesActivas;

    GameObject _prota;
	// Use this for initialization
	void Start () {
        _prota = GameObject.Find("Prota");
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(gameObject.transform.position, _prota.transform.position) < 3)
        {
            accion.enabled = true;
            accion.text = "E to examine Board";
        }
        else
            accion.enabled = false;
	}

    void mostrarMision(Text informacion, Image papel) {
        informacion.enabled = true;
        papel.enabled = true;
    }
}
