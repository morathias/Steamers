using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//===============================================================================================
public class Prota : MonoBehaviour {
    public float velocidad = 5f;
    float _angulo;
    float _stamina = 100;

    Vector3 _direccion = Vector3.zero;
    Vector3 _posicionDelMouse;
    Vector3 _posicionDelPlayer;             //en coordenadas de pantalla

    CharacterController _protaController;

    Stats _stats;

    public Image _barraVida;
    public Image _barraStamina;

    enum estados {      //para la maquina de estados
        moviendose,
        esquivando,
        muriendo,
        explosion,
        hablando
    }
    estados _estado = estados.moviendose;

    float _timerEsquivar = 0.2f;
    //-------------------------------------------------------------------------------------------
	void Start () {
        _stats = GetComponent<Stats>();
        _protaController = GetComponent<CharacterController>();
	}
    //-------------------------------------------------------------------------------------------
	void Update () {
        _barraVida.fillAmount = (float)_stats.vida / _stats.health;
        switch (_estado) {
            case estados.moviendose:
                moverProta();
                rotarProta();

                if (_stamina <= 100)
                    recuperarStamina();

                if (Input.GetKeyDown(KeyCode.Space) && _stamina > 25 && _estado != estados.hablando){
                    perderStamina();
                    _estado = estados.esquivando;
                }
                break;

            case estados.esquivando:
                esquivar();
                
                _timerEsquivar -= Time.deltaTime;
                if (_timerEsquivar <= 0)
                {
                    _estado = estados.moviendose;
                    _timerEsquivar = 0.2f;
                }
                break;

            case estados.explosion:
                _estado = estados.moviendose;
                break;

            case estados.hablando:
                //prota: -wololo-;
                break;
        }
        //Debug.Log(_estado);
	}
    //-------------------------------------------------------------------------------------------
    void moverProta() {
        _direccion = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _direccion *= velocidad;

        _protaController.SimpleMove(_direccion);
    }
    //-------------------------------------------------------------------------------------------
    void esquivar() {
        _protaController.SimpleMove(_direccion * 3f);
    }
    //-------------------------------------------------------------------------------------------
    void rotarProta() {
        _posicionDelMouse = Input.mousePosition;    //agarro la posicion del mouse
        _posicionDelMouse.z = 13f;  //distancia de la camara al prota

        _posicionDelPlayer = Camera.main.WorldToScreenPoint(transform.position);    //convierto la posicion del prota a screen

        _posicionDelMouse.x -= _posicionDelPlayer.x;    //saco la direccion
        _posicionDelMouse.y -= _posicionDelPlayer.y;    //_|

        _angulo = Mathf.Atan2(_posicionDelMouse.x, _posicionDelMouse.y) * Mathf.Rad2Deg;    //saco el angulo de ese vector
        transform.rotation = Quaternion.Euler(new Vector3(0, _angulo, 0));      //lo roto en eje y
    }
    //-------------------------------------------------------------------------------------------
    public void stunE() {
        _estado = estados.explosion;
    }
    //-------------------------------------------------------------------------------------------
    void perderStamina() {
        _stamina -= 25;
        _barraStamina.fillAmount = _stamina / 100f;
    }
    //-------------------------------------------------------------------------------------------
    void recuperarStamina() {
        _stamina += 10f * Time.deltaTime;
        _barraStamina.fillAmount = _stamina / 100f;
    }
    //-------------------------------------------------------------------------------------------
    public void estaHablando(bool hablando) {
        if (hablando)
            _estado = estados.hablando;
    }

    public void terminoDeHablar() {
        _estado = estados.moviendose;
    }
}
//===============================================================================================
