using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//===============================================================================================
public class Prota : MonoBehaviour {
    public float velocidad = 5f;
    float _angulo;
    int _stamina = 100;

    Vector3 _direccion = Vector3.zero;
    Vector3 _posicionDelMouse;
    Vector3 _posicionDelPlayer;             //en coordenadas de pantalla
    Vector3 _posicionEsquivar;

    CharacterController _protaController;

    Stats _stats;

    public Image _barraVida;
    public Image _barraStamina;

    enum estados {      //para la maquina de estados
        moviendose,
        esquivando,
        muriendo,
        explosion
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
        switch (_estado) {
            case estados.moviendose:
                moverProta();
                rotarProta();

                if (Input.GetKeyDown(KeyCode.Space) && _stamina > 0){
                    perderStamina();
                    _posicionEsquivar = new Vector3((_direccion.x * 1.2f) + transform.position.x,
                                                    transform.position.y, 
                                                    (_direccion.z * 1.2f) + transform.position.z);
                    _estado = estados.esquivando;
                }
                break;

            case estados.esquivando:
                esquivar();
                perderVidaTest();
                
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
        }
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

    public void stunE() {
        _estado = estados.explosion;
    }

    void perderVidaTest() {
        _stats.health -= 1;
        _barraVida.fillAmount = _stats.health / 100f;
    }

    void perderStamina() {
        _stamina -= 10;
        _barraStamina.fillAmount = _stamina / 100f;
    }
}
//===============================================================================================
