using UnityEngine;
using System.Collections;
//=================================================
public class Arma : MonoBehaviour {
    public int balas;
    public float delay;     //lo que tarda en volver a disparar, evita spameo
    public float daño;
    public float duracion; //tiempo que tarda en destruirse

    bool _recargaPerfecta;      //para la barrita que aumenta el daño temporalmente
    float _rangoRecarga = 1f;    //rango de la barrita;
    float _minPerfecto = 0.40f;
    float _maxPerfecto = 0.60f;
    int _balasActuales;

    enum estados {
        llena,
        vacia,
        recargando
    }
    estados _estado;
    //---------------------------------------------
	void Start () {
        _balasActuales = balas;
        _estado = estados.llena;
	}
    //---------------------------------------------
	void Update () {
        switch (_estado) {
            case estados.llena:
                if (Input.GetButton("Fire1")) {
                    Debug.Log(_balasActuales);
                    if (!disparar())
                        _estado = estados.vacia;
                }

                if (Input.GetKeyDown(KeyCode.R))
                    _estado = estados.recargando;
                break;

            case estados.vacia:
                if (Input.GetButtonDown("Fire1")) {
                    Debug.Log("No se puede");
                }

                if (Input.GetKeyDown(KeyCode.R))
                    _estado = estados.recargando;
                break;

            case estados.recargando:
                recargar();
                _estado = estados.llena;
                break;
        }
	}
    //---------------------------------------------
    bool disparar() {
        if (_balasActuales == 0) return false;

        else{
            Debug.Log("disparo");
            _balasActuales--;
            return true;
        }
    }
    //---------------------------------------------
    void recargar() {
        Debug.Log("recargando");
        _balasActuales = balas;
        Debug.Log(_balasActuales);
    }
    //---------------------------------------------
}
//=================================================
