using UnityEngine;
using System.Collections;
//===============================================================================================
public class Prota : MonoBehaviour {
    public float velocidad = 5f;
    float _angulo;
    
    Vector3 _direccion = Vector3.zero;
    Vector3 _posicionDelMouse;
    Vector3 _posicionDelPlayer;             //en coordenadas de pantalla
    Vector3 _posicionEsquivar;

    CharacterController _protaController;

    enum estados {      //para la maquina de estados
        moviendose,
        esquivando,
<<<<<<< HEAD
        muriendo
=======
        muriendo,
        explosion
>>>>>>> refs/remotes/origin/master
    }
    estados _estado = estados.moviendose;

    float _timerEsquivar = 0.5f;
    //-------------------------------------------------------------------------------------------
	void Start () {
        _protaController = GetComponent<CharacterController>();
	}
    //-------------------------------------------------------------------------------------------
	void Update () {
        switch (_estado) { 
            case estados.moviendose:
                moverProta();
                rotarProta();

                if (Input.GetKeyDown(KeyCode.Space)){
                    _posicionEsquivar = new Vector3((_direccion.x * 1.2f) + transform.position.x,
                                                    transform.position.y, 
                                                    (_direccion.z * 1.2f) + transform.position.z);
                    _estado = estados.esquivando;
                }
                break;

            case estados.esquivando:
                esquivar();

                _timerEsquivar -= Time.deltaTime;
                if (_timerEsquivar <= 0){
                    _estado = estados.moviendose;
                    _timerEsquivar = 0.5f;
                }
                break;
<<<<<<< HEAD
=======

            case estados.explosion:

                _estado = estados.moviendose;
                break;
>>>>>>> refs/remotes/origin/master
        }
	}
    //-------------------------------------------------------------------------------------------
    void moverProta() {
        _direccion = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _direccion *= velocidad;

        _protaController.Move(_direccion * Time.deltaTime);
    }
    //-------------------------------------------------------------------------------------------
    void esquivar() {
<<<<<<< HEAD
=======
        Debug.Log(_direccion);
>>>>>>> refs/remotes/origin/master
        transform.position = Vector3.LerpUnclamped(transform.position, _posicionEsquivar, 
                                                   Time.deltaTime * velocidad);
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
<<<<<<< HEAD
=======
    public void stunE() {
        _estado = estados.explosion;
    }
>>>>>>> refs/remotes/origin/master
}
//===============================================================================================
