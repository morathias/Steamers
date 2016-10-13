using UnityEngine;
using System.Collections;
//==============================================================================================================
public class Camara : MonoBehaviour {
    public Transform target;
    public float suavizado;
    public float altura;
    public float distancia;

    Vector3 _posicionFinal;
    //----------------------------------------------------------------------------------------------------------
	void Update () {
        if (!target)
            return;

        _posicionFinal = new Vector3(target.position.x, altura, target.position.z - distancia);
        transform.position = Vector3.Lerp(transform.position, _posicionFinal, Time.deltaTime * suavizado);
	}
    //----------------------------------------------------------------------------------------------------------
}
//==============================================================================================================
