using UnityEngine;
using System.Collections;

public class Camara : MonoBehaviour {
    public Transform target;
    public float suavizado;
    public float altura;
    public float distancia;

	void Update () {
        if (!target)
            return;

        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * suavizado);
        transform.position = new Vector3(transform.position.x, altura, transform.position.z);
	}
}
