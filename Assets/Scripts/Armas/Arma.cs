using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//=================================================
public class Arma : MonoBehaviour {
    public int balas;
    public float daño;
    public float distancia; //tiempo que tarda en destruirse
    int health;

    public Text balasTxt;

    bool _recargaPerfecta;      //para la barrita que aumenta el daño temporalmente
    bool _puedeDisparar = true;
    float _rangoRecarga = 1f;    //rango de la barrita;
    float _minPerfecto = 0.40f;
    float _maxPerfecto = 0.60f;
    protected int _balasActuales;

    public ParticleSystem _bala;

    protected enum estados {
        llena,
        vacia,
        recargando
    }
    protected estados _estado;
    //---------------------------------------------
	void Start () {
        _balasActuales = balas;
        _estado = estados.llena;
        balasTxt.text = "Balas: " + _balasActuales;
	}
    //---------------------------------------------
	void Update () {
        switch (_estado) {
            case estados.llena:
                if (!disparar())
                    _estado = estados.vacia;

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
    public virtual bool disparar() { return false; }
    //---------------------------------------------
    void recargar() {
        _balasActuales = balas;
    }
    //---------------------------------------------
}
//=================================================
