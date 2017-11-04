using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatOS : Overlord
{
    Transform fichador;
    public int Rango = 1;
    float intervalo = 0.2f;
    float prox = 0.0f;
    int limite = 0;
    int cool = 0;
    int cartucho = 0;
    int move = 0;
    Transform capitanPos;
    public ParticleSystem _balaE;
    Quaternion neededRotation;



    override protected void Start()
    {
        base.Start();
        _balaE.GetComponent<DañoBalas>().setDaño(daño);
        GameObject objective = GameObject.FindGameObjectWithTag("Player");
        fichador = objective.transform;
        _estado = estados.Durazno;

    }
    void Update()
    {
        Debug.Log("Bollocks");
        switch (_estado)
        {
            case estados.normal:

                if (dead == true)
                {
                    Destroy(gameObject);
                }

                if (Vector3.Distance(transform.position, fichador.position) < Rango)
                {
                    limite++;
                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                    neededRotation.x = 0;
                    neededRotation.z = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 2.5f);

                    RaycastHit ICU;

                    if (Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
                        _estado = estados.rage;
                    Debug.Log("Bollocks");

                }


                break;

            case estados.recarga:
                cool++;
                Debug.Log("Relaad!");
                if (cool > 90)
                {
                    cartucho = 0;
                    cool = 0;
                    limite = 0;
                    _estado = estados.normal;
                }
                break;

            case estados.rage:
                transform.LookAt(fichador);
                if (Time.deltaTime >= prox && cartucho < 10)
                {

                    prox = Time.deltaTime + intervalo;
                    _balaE.startLifetime = Rango / _balaE.startSpeed;
                    _balaE.transform.position = transform.position;
                    _balaE.Emit(1);
                    cartucho++;
                    Debug.Log(cartucho);
                }

                if (cartucho >= 10)
                    _estado = estados.recarga;

                break;

            case estados.Durazno:
                cool++;
                Debug.Log("Relaad!");
                if (cool > 90)
                {
                    cool = 0;
                    _estado = estados.normal;
                }
                break;
        }
    }

    public void Activate()
    {
        _estado = estados.Durazno;
    }
}

