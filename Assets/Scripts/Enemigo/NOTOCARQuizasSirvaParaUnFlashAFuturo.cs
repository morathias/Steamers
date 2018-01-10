using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOTOCARQuizasSirvaParaUnFlashAFuturo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
/////

//          if (dead)
//        {
//            Destroy(gameObject);
//        }
//        switch (_estado)
//        {
//            case estados.brag:
//                if (bossSat()=="1")
//                {

//                    limite += Time.deltaTime* Time.timeScale;

//                    if (limite>3 && transform.position != specific)
//                    {
//                        transform.position = Vector3.MoveTowards(transform.position, specific, 3 * Time.deltaTime);
//                    }
//                    if (transform.position == specific)
//                    {
//                        _animations.Play("Armature|shouting");
//                        limite = 0;
//                        _estado = estados.normal;
//                    }
//                }
//                break;

//            case estados.normal:
//                _animations.Play("Armature|idle");
//                limite += Time.deltaTime* Time.timeScale;
//                if (range)
//                {
//                    if (Vector3.Distance(transform.position, fichador.position) < Rango)
//                    {
//                        neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
//                        neededRotation.x = 0;
//                        neededRotation.z = 0;
//                        transform.LookAt(fichador);
//                        _animations.Play("Armature|shooting");
//                        //	transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 2.3f);

//                        if (limite > random)
//                            fire();

//                    }
//                    else
//                        _animations.Play("Armature|idle");

//                }
//                else
//                {
//                    if (Vector3.Distance(transform.position, Target.position) < RangoBusqueda)
//                    {
//                        _animations.Play("Armature|running");
//                        transform.LookAt(Target.position);
//                        transform.Translate(Vector3.forward* 3 * Time.deltaTime);
//                    }
//                    else
//                        _animations.Play("Armature|idle");

//                    if (Vector3.Distance(transform.position, Target.position) < RangoRush && limite > 1)
//                    {
//                        limite = 0;
//                        _animations.Play("Armature|idle");
//                        _estado = estados.apuntando;
//                    }

//                }  

//                break;

//            case estados.rage:
//                _animations.Play("Armature|running");
//                transform.Translate(Vector3.forward* velocidad * Time.deltaTime);
//limite += Time.deltaTime* Time.timeScale;
//velocidad += 1;
//                if (limite > 0.8)
//                {
//                    velocidad = 0;
//                    limite = 0;
//                    cool = 1;
//                    _estado = estados.Durazno;
//                }

//                break;

//            case estados.Durazno:
//                _animations.Play("Armature|idle");
//                limite += Time.deltaTime* Time.timeScale;
//random2 = Random.Range(1, 10);
//                if (random2 > 5)
//                {
//                    range = true;
//                }
//                else
//                    range = false;
//                if (limite > cool)
//                {
//                    limite = 0;
//                    _estado = estados.normal;
//                }
//                random = Random.Range(0.5f, 1.5f);

//                break;

//            case estados.apuntando:
//                transform.LookAt(Target.position);

//                RaycastHit ICU;

//                if (Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
//                    _estado = estados.rage;
//                Debug.Log("Bollocks");
//                break;


//            case estados.recarga:
//                cool++;
//                Debug.Log("Relaad!");
//                //if (random2 > 5)
//                //{
//                //    range = true;
//                //}
//                //else
//                    range = false;
//                if (cool > 90)
//                {
//                    cartucho = 0;
//                    cool = 0;
//                    limite = 0;
//                    _estado = estados.normal;
//                    random = Random.Range(40, 90);
//                }
//                break;
//        }
//    }


//    void fire()
//{
//    if (Time.time >= prox && cartucho < 20)
//    {

//        prox = Time.time + intervalo;
//        _balaE.startLifetime = Rango / _balaE.startSpeed;
//        //_balaE.transform.position = transform.position;
//        _balaE.transform.LookAt(objective.transform);
//        _balaE.Emit(1);
//        cartucho++;
//        Debug.Log(cartucho);
//    }

//    if (cartucho >= 20)
//        _estado = estados.recarga;

//}

//void OnCollisionEnter(Collision collision)
//{
//    if (collision.gameObject.tag == "Player")
//    {

//        Stats healthComponent = collision.gameObject.GetComponent<Stats>();
//        healthComponent.applyDamage(daño);

//        stayput = true;
//    }
//}