using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trooper : Overlord
{
    Transform fichador;
    public int Rango = 1;
    float intervalo = 0.2f;
    float prox = 0.0f;
    int limite = 0;
    int cool = 0;
    int cartucho = 0;
    int move = 0;
    int random = 0;
    public ParticleSystem _balaE;
    Quaternion neededRotation;
    //  private int ordenPos;
    Vector3 _posicionLider;


    override protected void Start()
    {
        base.Start();
        _balaE.GetComponent<DañoBalas>().setDaño(daño);
        GameObject objective = GameObject.FindGameObjectWithTag("Player");
        fichador = objective.transform;

        random = Random.Range(40, 90);

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
                    transform.LookAt(fichador);
                    //	transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 2.3f);

                    if (limite > random)
                        fire();

                    moveIt();

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
                    random = Random.Range(40, 90);
                }
                break;
        }
    }


    void fire()
    {
        if (Time.time >= prox && cartucho < 20)
        {

            prox = Time.time + intervalo;
            _balaE.startLifetime = Rango / _balaE.startSpeed;
            _balaE.transform.position = transform.position;
            _balaE.Emit(1);
            cartucho++;
            Debug.Log(cartucho);
        }

        if (cartucho >= 20)
            _estado = estados.recarga;

    }

    void moveIt()
    {
        move++;
        if (move > 90)
        {
            _posicionLider = transform.position;
            _posicionLider.x += Random.Range(-20.0f, 20.0f);
            _posicionLider.z += Random.Range(-20.0f, 20.0f);
            move = 0;

        }
        transform.position = Vector3.MoveTowards(transform.position, _posicionLider, 2 * Time.deltaTime);

    }
}
