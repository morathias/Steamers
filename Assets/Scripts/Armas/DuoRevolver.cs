using UnityEngine;
using System.Collections;

public class DuoRevolver : Arma {
    Transform _revolverIzquierdo, _revolverDerecho;
    int _cambiarArma = 0;

    public Animator protaAnimations;

    override protected void Start() {
        _revolverIzquierdo = transform.Find("revolver_Izquierdo");
        _revolverDerecho = transform.Find("revolver_Derecho");

        base.Start();
    }

    public override bool disparar() {
        _bala.startLifetime = distancia / _bala.startSpeed;
        if (Input.GetButtonDown("Fire1")) {
            if(_balasActuales <= 0)  return false;

            else{
                _bala.Emit(1);
                if (_cambiarArma == 0){
                    protaAnimations.Play("Armature|shoot_R");
                    _cambiarArma = 1;
                    _bala.transform.position = _revolverDerecho.position;
                }
                else {
                    protaAnimations.Play("Armature|shoot_L");
                    _cambiarArma = 0;
                    _bala.transform.position = _revolverIzquierdo.position;
                }

                _balasActuales--;
                return true;
            }
        }
        return true;
    }
}
