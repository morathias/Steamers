using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//=================================================
public class Arma : MonoBehaviour {
    public int balas;
    public float delay;     //lo que tarda en volver a disparar, evita spameo
    public float daño;
    public float duracion; //tiempo que tarda en destruirse
    int health;

    public Text balasTxt;

    bool _recargaPerfecta;      //para la barrita que aumenta el daño temporalmente
    bool _puedeDisparar = true;
    float _rangoRecarga = 1f;    //rango de la barrita;
    float _minPerfecto = 0.40f;
    float _maxPerfecto = 0.60f;
    int _balasActuales;
    float _delayActual;

    enum estados {
        llena,
        vacia,
        recargando
    }
    estados _estado;
    //---------------------------------------------
	void Start () {
        _balasActuales = balas;
        _delayActual = delay;
        _estado = estados.llena;
        balasTxt.text = "Balas: " + _balasActuales;
	}
    //---------------------------------------------
	void Update () {
        switch (_estado) {
            case estados.llena:
                if (Input.GetButton("Fire1") && _puedeDisparar) {
                    _puedeDisparar = false;
                    if (!disparar())
                        _estado = estados.vacia;
                }

                if (!_puedeDisparar)
                    _delayActual -= Time.deltaTime;
                if (_delayActual <= 0) {
                    _delayActual = delay;
                    _puedeDisparar = true;
                }

                //chequea que no este llena, evita recargar estando llena
                if (Input.GetKeyDown(KeyCode.R) && _balasActuales != balas) 
                    _estado = estados.recargando;
                break;

            case estados.vacia:
                if (Input.GetKeyDown(KeyCode.R))
                    _estado = estados.recargando;
                break;

            case estados.recargando:
                recargar();
                _estado = estados.llena;
                break;
        }

        balasTxt.text = "Balas: " + _balasActuales;
	}
    //---------------------------------------------
    bool disparar() {
        if (_balasActuales <= 0) return false;

        else{
            _balasActuales--;
            return true;
        }
    }
    //---------------------------------------------
    void recargar() {
        _balasActuales = balas;
    }
    //---------------------------------------------
}
//=================================================
