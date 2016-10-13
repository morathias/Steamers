using UnityEngine;
using System.Collections;
//===============================================================================================
public class Prota : MonoBehaviour {
    public float velocidad = 5f;
    float _angulo;
    
    Vector3 _direccion = Vector3.zero;
    Vector3 _posicionDelMouse;
    Vector3 _posicionDelPlayer;             //en coordenadas de pantalla

    CharacterController _protaController;

    enum estados {      //para la maquina de estados
        moviendose,
        esquivando,
        muriendo
    }
    estados _estado = estados.moviendose;
    //-------------------------------------------------------------------------------------------
	void Start () {
        _protaController = GetComponent<CharacterController>();
	}
    //-------------------------------------------------------------------------------------------
	void Update () {
        switch (_estado) { 
            case estados.moviendose:
                _direccion = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                _direccion *= velocidad;

                _protaController.Move(_direccion * Time.deltaTime);

                rotarProta();
                break;
        }
	}
    //-------------------------------------------------------------------------------------------
    void rotarProta() {
        _posicionDelMouse = Input.mousePosition;    //agarro la posicion del mouse
        _posicionDelMouse.z = 13f;  //distancia de la camara al prota

        _posicionDelPlayer = Camera.main.WorldToScreenPoint(transform.position);    //convierto la posicion del player a screen

        _posicionDelMouse.x -= _posicionDelPlayer.x;    //saco la direccion
        _posicionDelMouse.y -= _posicionDelPlayer.y;    //_|

        _angulo = Mathf.Atan2(_posicionDelMouse.x, _posicionDelMouse.y) * Mathf.Rad2Deg;    //saco el angulo de ese vector
        transform.rotation = Quaternion.Euler(new Vector3(0, _angulo, 0));      //lo roto en eje y
    }
}
//===============================================================================================
