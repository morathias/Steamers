using UnityEngine;
using System.Collections;
//==================================================================
public class Prota : MonoBehaviour {
    public float velocidad = 5f;
    
    Vector3 _direccion = Vector3.zero;
    CharacterController _protaController;
    //--------------------------------------------------------------
	void Start () {
        _protaController = GetComponent<CharacterController>();
	}
    //--------------------------------------------------------------
	void Update () {
        _direccion = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _direccion = transform.TransformDirection(_direccion);
        _direccion *= velocidad;

        _protaController.Move(_direccion * Time.deltaTime);
	}
    //--------------------------------------------------------------
}
