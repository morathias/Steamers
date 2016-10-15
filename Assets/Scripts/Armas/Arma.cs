using UnityEngine;
using System.Collections;
<<<<<<< HEAD
using UnityEngine.UI;
=======
>>>>>>> refs/remotes/origin/master
//=================================================
public class Arma : MonoBehaviour {
    public int balas;
    public float delay;     //lo que tarda en volver a disparar, evita spameo
    public float daño;
    public float duracion; //tiempo que tarda en destruirse

<<<<<<< HEAD
    public Text balasTxt;

    bool _recargaPerfecta;      //para la barrita que aumenta el daño temporalmente
    bool _puedeDisparar = true;
=======
    bool _recargaPerfecta;      //para la barrita que aumenta el daño temporalmente
>>>>>>> refs/remotes/origin/master
    float _rangoRecarga = 1f;    //rango de la barrita;
    float _minPerfecto = 0.40f;
    float _maxPerfecto = 0.60f;
    int _balasActuales;
<<<<<<< HEAD
    float _delayActual;
=======
>>>>>>> refs/remotes/origin/master

    enum estados {
        llena,
        vacia,
        recargando
    }
    estados _estado;
    //---------------------------------------------
	void Start () {
        _balasActuales = balas;
<<<<<<< HEAD
        _delayActual = delay;
        _estado = estados.llena;
        balasTxt.text = "Balas: " + _balasActuales;
=======
        _estado = estados.llena;
>>>>>>> refs/remotes/origin/master
	}
    //---------------------------------------------
	void Update () {
        switch (_estado) {
            case estados.llena:
<<<<<<< HEAD
                if (Input.GetButton("Fire1") && _puedeDisparar) {
                    _puedeDisparar = false;
=======
                if (Input.GetButton("Fire1")) {
                    Debug.Log(_balasActuales);
>>>>>>> refs/remotes/origin/master
                    if (!disparar())
                        _estado = estados.vacia;
                }

<<<<<<< HEAD
                if (!_puedeDisparar)
                    _delayActual -= Time.deltaTime;
                if (_delayActual <= 0) {
                    _delayActual = delay;
                    _puedeDisparar = true;
                }

                //chequea que no este llena, evita recargar estando llena
                if (Input.GetKeyDown(KeyCode.R) && _balasActuales != balas) 
=======
                if (Input.GetKeyDown(KeyCode.R))
>>>>>>> refs/remotes/origin/master
                    _estado = estados.recargando;
                break;

            case estados.vacia:
<<<<<<< HEAD
=======
                if (Input.GetButtonDown("Fire1")) {
                    Debug.Log("No se puede");
                }

>>>>>>> refs/remotes/origin/master
                if (Input.GetKeyDown(KeyCode.R))
                    _estado = estados.recargando;
                break;

            case estados.recargando:
                recargar();
                _estado = estados.llena;
                break;
        }
<<<<<<< HEAD

        balasTxt.text = "Balas: " + _balasActuales;
	}
    //---------------------------------------------
    bool disparar() {
        if (_balasActuales <= 0) return false;

        else{
=======
	}
    //---------------------------------------------
    bool disparar() {
        if (_balasActuales == 0) return false;

        else{
            Debug.Log("disparo");
>>>>>>> refs/remotes/origin/master
            _balasActuales--;
            return true;
        }
    }
    //---------------------------------------------
    void recargar() {
<<<<<<< HEAD
        _balasActuales = balas;
=======
        Debug.Log("recargando");
        _balasActuales = balas;
        Debug.Log(_balasActuales);
>>>>>>> refs/remotes/origin/master
    }
    //---------------------------------------------
}
//=================================================
