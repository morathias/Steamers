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
    float _rangoRecarga = 0f;    //rango de la barrita;
    float _minPerfecto = 0.54f;
    float _maxPerfecto = 0.61f;
    protected int _balasActuales;

    public ParticleSystem _bala;

    public Image barraRecarga;
    public Image barraRecargaVacia;
    public Image barraRecargaPunto;

    protected enum estados {
        llena,
        recargando
    }
    protected estados _estado;
    //---------------------------------------------
	void Start () {
        _balasActuales = balas;
        _estado = estados.llena;
        balasTxt.text = "Balas: " + _balasActuales;
        barraRecarga.enabled = false;
        barraRecargaVacia.enabled = false;
        barraRecargaPunto.enabled = false;
	}
    //---------------------------------------------
	void Update () {
        switch (_estado) {
            case estados.llena:
                if (!disparar())
                    _estado = estados.recargando;

                //chequea que no este llena, evita recargar estando llena
                if (Input.GetKeyDown(KeyCode.R) && _balasActuales != balas) 
                    _estado = estados.recargando;
                break;

            case estados.recargando:
                barraRecarga.enabled = true;
                barraRecargaVacia.enabled = true;
                barraRecargaPunto.enabled = true;

                if (recargar()){
                    _estado = estados.llena;
                    barraRecarga.enabled = false;
                    barraRecargaVacia.enabled = false;
                    barraRecargaPunto.enabled = false;
                }
                break;
        }

        balasTxt.text = "Balas: " + _balasActuales;
	}
    //---------------------------------------------
    public virtual bool disparar() { return false; }
    //---------------------------------------------
    bool recargar() {
        _rangoRecarga += 0.01f;
        barraRecarga.fillAmount = _rangoRecarga;

        if (Input.GetKeyDown(KeyCode.R)){
            if (_rangoRecarga >= _minPerfecto && _rangoRecarga <= _maxPerfecto)
            {
                _rangoRecarga = 0f;
                barraRecarga.fillAmount = _rangoRecarga;
                _balasActuales = balas;
                daño *= 2;
                return true;
            }
            else {
                _rangoRecarga = 0f;
                barraRecarga.fillAmount = _rangoRecarga;
                return false;
            }
        }

        if (_rangoRecarga >= 1f){
            _rangoRecarga = 0f;
            barraRecarga.fillAmount = _rangoRecarga;
            _balasActuales = balas;
            return true;
        }
        return false;
    }
    //---------------------------------------------
}
//=================================================
