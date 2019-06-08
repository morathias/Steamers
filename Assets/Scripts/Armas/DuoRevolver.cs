using UnityEngine;
using System.Collections;

public class DuoRevolver : Arma {
    Vector3 _revolverIzquierdo, _revolverDerecho;
    int _cambiarArma = 0;

    public AudioClip shootSound;
    public AudioSource shootSource;
    public float vol = 1f;
    public Animator protaAnimations;

    public LayerMask layer;

    override protected void Start() {
        _revolverIzquierdo = new Vector3(-0.486f, 0.805f, 1.561f);
        _revolverDerecho = new Vector3(0.486f, 0.805f, 1.561f);

        base.Start();
    }
    Vector3 mouseToWorld()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layer))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
    public override bool disparar() {
        _bala.startLifetime = distancia / _bala.startSpeed;
        if (Input.GetButtonDown("Fire1")) {
            if(_balasActuales <= 0)  return false;

            else{
                Vector3 target = mouseToWorld();
                if (target != Vector3.zero)
                    _bala.transform.LookAt(mouseToWorld());
                else
                    _bala.transform.rotation = transform.rotation;
                _bala.Emit(1);
                shootSource.PlayOneShot(shootSound,vol);
                _smoke.Play();
                _sparks.Play();
                if (_cambiarArma == 0){
                    protaAnimations.Play("shoot_l",1);
                    _cambiarArma = 1;
                    _bala.transform.localPosition = _revolverDerecho;
                    _smoke.transform.localPosition = _revolverDerecho;
                    _sparks.transform.localPosition = _revolverDerecho;
                }
                else {
                    protaAnimations.Play("shoot_r",1);
                    _cambiarArma = 0;
                    _bala.transform.localPosition = _revolverIzquierdo;
                    _smoke.transform.localPosition = _revolverIzquierdo;
                    _sparks.transform.localPosition = _revolverIzquierdo;

                }

                _balasActuales--;
                return true;
            }
        }
        return true;
    }
}
