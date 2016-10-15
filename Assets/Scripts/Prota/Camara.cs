using UnityEngine;
using System.Collections;
//==============================================================================================================
public class Camara : MonoBehaviour {
    public Transform target;
    public float suavizado;
    public float altura;
    public float distancia;
    float alturaActual;

    Vector3 _posicionFinal;

    void Start() {
        alturaActual = altura;
    }
    //----------------------------------------------------------------------------------------------------------
	void Update () {
        if (!target)
            return;

        transform.LookAt(target);

        _posicionFinal = new Vector3(target.position.x, alturaActual, target.position.z - distancia);
        transform.position = Vector3.Lerp(transform.position, _posicionFinal, Time.deltaTime * suavizado);

        zoom();
	}
    //----------------------------------------------------------------------------------------------------------
    void zoom() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0){
            alturaActual-= 0.5f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            alturaActual+= 0.5f;
        }

        if (alturaActual <= 3f)
            alturaActual = 5f;
        if (alturaActual >= altura)
            alturaActual = altura;
    }
}
//==============================================================================================================
