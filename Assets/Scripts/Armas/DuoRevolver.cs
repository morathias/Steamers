using UnityEngine;
using System.Collections;

public class DuoRevolver : Arma {
    Transform _revolverIzquierdo, _revolverDerecho;
    int _cambiarArma = 0;

    override protected void Start() {
        _revolverIzquierdo = transform.FindChild("revolver_Izquierdo");
        _revolverDerecho = transform.FindChild("revolver_Derecho");

        base.Start();
    }

    public override bool disparar() {
        _bala.startLifetime = distancia / _bala.startSpeed;
        if (Input.GetButtonDown("Fire1")) {
            if(_balasActuales <= 0)  return false;

            else{
                _bala.Emit(1);
                if (_cambiarArma == 0){
                    _cambiarArma = 1;
                    _bala.transform.position = _revolverDerecho.position;
                }
                else {
                    _cambiarArma = 0;
                    _bala.transform.position = _revolverIzquierdo.position;
                }

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, distancia)){
                    //Debug.Log("choco bala");
                }

                _balasActuales--;
                return true;
            }
        }
        return true;
    }
}
