using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//=================================================
public class Arma : MonoBehaviour
{
    public int balas;
    float _daño;
    public float distancia; //tiempo que tarda en destruirse
    private Stats statsComponent;

    public Text balasTxt;

    bool _recargaPerfecta;      //para la barrita que aumenta el daño temporalmente
    float _rangoRecarga = 0f;    //rango de la barrita;
    float _minPerfecto = 0.54f;
    float _maxPerfecto = 0.61f;
    protected int _balasActuales;
    public float _timerReload = 0f;

    public ParticleSystem _bala;
    protected DañoBalas _dañoBala;

    public Image barraRecarga;
    public Image barraRecargaVacia;
    public Image barraRecargaPunto;

    Prota _prota;

    protected enum estados
    {
        llena,
        recargando
    }
    protected estados _estado;
    //---------------------------------------------
    protected virtual void Start()
    {
        statsComponent = GameObject.Find("Prota").gameObject.GetComponent<Stats>();

        _daño = statsComponent.damageFinal;
        _dañoBala = _bala.GetComponent<DañoBalas>();
        _dañoBala.setDaño((int)_daño);

        _balasActuales = balas;
        _estado = estados.llena;

        balasTxt.text = "Balas: " + _balasActuales;
        barraRecarga.enabled = false;
        barraRecargaVacia.enabled = false;
        barraRecargaPunto.enabled = false;

        _prota = GameObject.Find("Prota").GetComponent<Prota>();
    }
    //---------------------------------------------
    void Update()
    {
        _daño = statsComponent.damageFinal;
        _dañoBala.setDaño((int)_daño);
        if (_recargaPerfecta){
            _timerReload += Time.deltaTime;
        }
        if (_prota.enabled)
        {
            switch (_estado)
            {
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

                    if (recargar())
                    {
                        _estado = estados.llena;
                        barraRecarga.enabled = false;
                        barraRecargaVacia.enabled = false;
                        barraRecargaPunto.enabled = false;
                    }
                    break;
            }
        }

        balasTxt.text = "Balas: " + _balasActuales;

        if (_timerReload >= 2.5f)
        {
            _recargaPerfecta = false;
            statsComponent.damage /= statsComponent.buffReload;
            _timerReload = 0;
        }
    }
    //---------------------------------------------
    public virtual bool disparar() { return false; }
    //---------------------------------------------
    bool recargar()
    {
        _rangoRecarga += 0.75f * Time.deltaTime;
        barraRecarga.fillAmount = _rangoRecarga;

        if (Input.GetKeyDown(KeyCode.R))
        {   if (_rangoRecarga < _minPerfecto){
                _rangoRecarga = 0f;
                barraRecarga.fillAmount = _rangoRecarga;
                return false;
            }
            else if (_rangoRecarga >= _minPerfecto && _rangoRecarga <= _maxPerfecto)
            {
                _rangoRecarga = 0f;
                barraRecarga.fillAmount = _rangoRecarga;
                _balasActuales = balas;
                if (!_recargaPerfecta){
                    statsComponent.damage *= statsComponent.buffReload;
                }
                _recargaPerfecta = true;
                return true;
            }
            else
            {
                _rangoRecarga = 0f;
                barraRecarga.fillAmount = _rangoRecarga;
                _balasActuales = balas;
                return true;
            }
        }

        if (_rangoRecarga >= 1f)
        {
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
