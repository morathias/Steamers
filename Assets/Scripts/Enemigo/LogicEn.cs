using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LogicEn : Overlord
{
    Transform fichador;
    public int Rango = 1;
    int limite = 0;
    int deploy = 0;
    int time = 0;
    int toss = 0;
    int thisThat = 0;
    int turret = 0;
    int move = 0;
    public ParticleSystem _balaE;
    Quaternion neededRotation;

    public GameObject Turret1;
    public GameObject Turret2;
    public GameObject Mine1;

    Vector3 _posicionLider;




    override protected void Start()
    {
        base.Start();
        _balaE.GetComponent<DañoBalas>().setDaño(daño);
        GameObject objective = GameObject.FindGameObjectWithTag("Player");
        fichador = objective.transform;
        deploy = Random.Range(240, 480);

    }
    void Update()
    {

        switch (_estado)
        {
            case estados.normal:
                time++;

                if (dead == true)
                {
                    Destroy(gameObject);
                }

                if (time > deploy && toss < 5)
                {
                    time = 0;
                    thisThat = Random.Range(0, 2);
                    _estado = estados.apuntando;
                }

                if (Vector3.Distance(transform.position, fichador.position) < Rango)
                {
                    limite++;
                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                    neededRotation.x = 0;
                    neededRotation.z = 0;

                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 2.0f);
                    if (limite > 110)
                        fire();
                    moveIt();

                }


                break;
            case estados.apuntando: //deploying

                if (turret < 2 && thisThat < 1)
                {
                    turret++;
                    Instantiate(Turret1, transform.position, transform.rotation);
                    Turret1.GetComponent<PotatOS>().Activate();
                }
                else
                {
                    Instantiate(Mine1, transform.position, transform.rotation);
                }
                toss++;
                deploy = Random.Range(240, 480);
                _estado = estados.normal;

                break;
        }

    }

    void fire()
    {
        _balaE.startLifetime = Rango / _balaE.startSpeed;

        _balaE.transform.position = transform.position;
        _balaE.Emit(30);

        limite = 0;
    }

    void moveIt()
    {
        move++;
        if (move > 150)
        {
            _posicionLider = transform.position;
            _posicionLider.x += Random.Range(-30.0f, 30.0f);
            _posicionLider.z += Random.Range(-30.0f, 30.0f);
            move = 0;

        }
        transform.position = Vector3.MoveTowards(transform.position, _posicionLider, 2 * Time.deltaTime);

    }
}