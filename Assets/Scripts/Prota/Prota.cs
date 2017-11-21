using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//===============================================================================================
public class Prota : MonoBehaviour
{
    public float velocidad = 5f;
    public GameObject Shield;
    public ParticleSystem FireL;
    public float staminaBase = 100;
    bool special = false;
    float _angulo;
    float _stamina = 100;
    public float consumoShield = 0.25f;
    public float consumoLlama = 0.25f;
    public float consumoDash = 25f;
    public bool ShieldCD = false;
    public bool FlameCD = false;
    public float ShieldTimer = 0;
    public float FlameTimer = 0;

    Vector3 _direccion = Vector3.zero;
    Vector3 _posicionDelMouse;
    Vector3 _posicionDelPlayer;             //en coordenadas de pantalla

    CharacterController _protaController;

    Stats _stats;

    public GameObject protaStatsUi;

    Image _barraVida;
    Image _barraVidaVacia;

    Image _barraStamina;
    Image _barraStaminaVacia;

    Image _barraExp;
    Image _panel;

    public Text hasMuertoTxt;
    public Text muertoReset;

    Animator _animations;

    enum estados
    {      //para la maquina de estados
        moviendose,
        esquivando,
        muriendo,
        explosion,
        hablando
    }
    estados _estado = estados.moviendose;

    float _timerEsquivar = 0.2f;

    //-------------------------------------------------------------------------------------------
    void Start()
    {
        _stats = GetComponent<Stats>();
        _protaController = GetComponent<CharacterController>();
        _animations = transform.Find("gratmos_animado").GetComponent<Animator>();

        _barraVida = protaStatsUi.transform.Find("life_bar").GetComponent<Image>();
        _barraVidaVacia = protaStatsUi.transform.Find("empty_bar").GetComponent<Image>();

        _barraStamina = protaStatsUi.transform.Find("stamina_bar").GetComponent<Image>();
        _barraStaminaVacia = protaStatsUi.transform.Find("empty_bar_stamina").GetComponent<Image>();

        _panel = protaStatsUi.transform.Find("panel").GetComponent<Image>();
        _barraExp = protaStatsUi.transform.Find("exp_bar").GetComponent<Image>();
    }
    //-------------------------------------------------------------------------------------------
    void Update()
    {
        updateBars();

        if (ShieldCD){
            ShieldTimer += Time.deltaTime;
            if(ShieldTimer > 1){
                ShieldCD = false;
                ShieldTimer = 0;
            }
        }
        if (FlameCD)
        {
            FlameTimer += Time.deltaTime;
            if (FlameTimer > 1)
            {
                FlameCD = false;
                FlameTimer = 0;
            }
        }

        if (_stats.VidaActual <= 0)
            _estado = estados.muriendo;

        switch (_estado)
        {
            case estados.moviendose:
                moverProta();
                rotarProta();

                //Debug.Log(_direccion.magnitude);
                if (_direccion.magnitude >= 1)
                    _animations.Play("Armature|running_front");
                else
                    _animations.Play("Armature|iddle");

                if (Stamina <= staminaBase && special == false)
                    recuperarStamina();

                if (Input.GetKeyDown(KeyCode.Space) && Stamina > 20 && _estado != estados.hablando)
                {
                    perderStamina(consumoDash);
                    _estado = estados.esquivando;

                }

                if (Input.GetKey(KeyCode.F) && !ShieldCD && _estado != estados.hablando)
                {
                    Shield.SetActive(true);
                    perderStamina(consumoShield);
                    special = true;
                    if (Stamina < 10)
                        ShieldCD = true;
                }
                else
                {
                    Shield.SetActive(false);
                    special = false;
                }

                if (!FlameCD &&  Input.GetKeyDown(KeyCode.LeftShift) && _estado != estados.hablando)
                {
                    FireL.Play(true);
                }

                else if (Input.GetKeyUp(KeyCode.LeftShift)|| FlameCD)
                {
                    FireL.Stop(true);
                    special = false;
                }
                if (Input.GetKey(KeyCode.LeftShift) && !FlameCD && _estado != estados.hablando)
                { 
                    perderStamina(consumoLlama);
                    special = true;
                    if (Stamina < 10)
                        FlameCD = true;
                }

                break;

            case estados.esquivando:
                esquivar();

                _timerEsquivar -= Time.deltaTime;
                if (_timerEsquivar <= 0)
                {
                    _estado = estados.moviendose;
                    _timerEsquivar = 0.4f;
                }
                break;

            case estados.explosion:
                _estado = estados.moviendose;
                break;

            case estados.hablando:

                //prota: -wololo-;
                break;

            case estados.muriendo:
                hasMuertoTxt.enabled = true;
                muertoReset.enabled = true;
                this.enabled = false;
                break;
        }
    }
    //-------------------------------------------------------------------------------------------
    void updateBars() {
        _barraVida.fillAmount = (float)(_stats.VidaActual / _stats.health) * _stats._porcentajeActualBarraVida;
        _barraVidaVacia.fillAmount = (float)(_stats.health / _stats.health) * _stats._porcentajeActualBarraVida;

        _barraStamina.fillAmount = (float)(Stamina / staminaBase) * _stats._porcentajeActualBarraStamina;
        _barraStaminaVacia.fillAmount = (float)(staminaBase/ staminaBase) * _stats._porcentajeActualBarraStamina;

        _barraExp.fillAmount = (float)_stats.exp / (100f * (1 + _stats.Levels * 0.5f));

        if (_stats._porcentajeActualBarraStamina > _stats._porcentajeActualBarraVida)
            _panel.fillAmount = _stats._porcentajeActualBarraStamina;
        else
            _panel.fillAmount = _stats._porcentajeActualBarraVida;
    }

    void moverProta()
    {
        _direccion = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _direccion *= velocidad;

        _protaController.SimpleMove(_direccion);
    }
    //-------------------------------------------------------------------------------------------
    void esquivar()
    {
        _protaController.SimpleMove(_direccion * 3.75f);
    }
    //-------------------------------------------------------------------------------------------
    void rotarProta()
    {
        _posicionDelMouse = Input.mousePosition;    //agarro la posicion del mouse
        _posicionDelMouse.z = 13f;  //distancia de la camara al prota

        _posicionDelPlayer = Camera.main.WorldToScreenPoint(transform.position);    //convierto la posicion del prota a screen

        _posicionDelMouse.x -= _posicionDelPlayer.x;    //saco la direccion
        _posicionDelMouse.y -= _posicionDelPlayer.y;    //_|

        _angulo = Mathf.Atan2(_posicionDelMouse.x, _posicionDelMouse.y) * Mathf.Rad2Deg;    //saco el angulo de ese vector
        transform.rotation = Quaternion.Euler(new Vector3(0, _angulo, 0));      //lo roto en eje y
    }
    //-------------------------------------------------------------------------------------------
    public void stunE()
    {
        _estado = estados.explosion;
    }
    //-------------------------------------------------------------------------------------------
    void perderStamina(float value)
    {
        Stamina -= value;
        _barraStamina.fillAmount = (Stamina / staminaBase) * _stats._porcentajeActualBarraStamina;
    }
    //-------------------------------------------------------------------------------------------
    void recuperarStamina()
    {
        Stamina += (10f + _stats.rage) * Time.deltaTime;
        _barraStamina.fillAmount = (Stamina / staminaBase) * _stats._porcentajeActualBarraStamina;
    }
    //-------------------------------------------------------------------------------------------
    public void estaHablando(bool hablando)
    {
        if (hablando)
            _estado = estados.hablando;
    }
    //-------------------------------------------------------------------------------------------
    public void terminoDeHablar()
    {
        _estado = estados.moviendose;
        Debug.Log(_estado);
    }
    //-------------------------------------------------------------------------------------------
    public void fear()
    {

    }
    //-------------------------------------------------------------------------------------------
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "BalaE" && _estado != estados.esquivando)
        {
            Debug.Log("ouch");
            Debug.Log(_stats.applyDamage(other.GetComponent<DañoBalas>().getDaño()));
            if (_stats.applyDamage(other.GetComponent<DañoBalas>().getDaño()))
                _estado = estados.muriendo;

            //_barraVida.fillAmount = _stats.VidaActual / _stats.health;
        }
    }
    //===============================================================================================
    public float Stamina
    {
        get
        {
            return _stamina;
        }

        set
        {
            _stamina = value;
        }
    }
}
//===============================================================================================
