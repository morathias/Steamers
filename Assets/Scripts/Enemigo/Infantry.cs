using UnityEngine;
using System.Collections;

public class Infantry : Overlord
{
    Transform fichador;
    public int Rango = 50;
    float intervalo = 0.2f;
    float prox = 0.0f;
    int limite = 60;
    int cool = 0;
    int move = 0;
    Transform capitanPos;
    public ParticleSystem _balaE;
    Quaternion neededRotation;
    private int ordenPos;

    Vector3 _posicionLider;

    enum estados
    {
        normal,
        bajoOrdenes
    }

    estados _estado = estados.normal;

    override protected void Start()
    {
        base.Start();
        _stats.applyDamage(1);
        GameObject objective = GameObject.FindGameObjectWithTag("Player");
        fichador = objective.transform;

        GameObject Leader = GameObject.FindGameObjectWithTag("Capitan");
        capitanPos = Leader.transform;

    }
    void Update()
    {
        switch (_estado)
        {
            case estados.normal:

                if (Vector3.Distance(transform.position, fichador.position) < Rango)
                {
                    limite++;
                    transform.LookAt(fichador.position);
                    if (limite > 80)
                        fire();
                    moveIt();

                }


                break;

            case estados.bajoOrdenes:

                switch (ordenPos)
                {
                    case 0:
                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 3);
                        break;
                    case 1:
                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 3) - (capitanPos.right * 3);

                        break;
                    case 2:
                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 3) - (capitanPos.right * -3);

                        break;
                    case 3:
                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 6);

                        break;
                    case 4:
                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 6) - (capitanPos.right * 3);

                        break;
                    case 5:
                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 6) - (capitanPos.right * -3);
                        break;

                }

                transform.position = Vector3.MoveTowards(transform.position, _posicionLider, 5 * Time.deltaTime);



                if (Vector3.Distance(transform.position, fichador.position) < Rango)
                {
                    limite++;
                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                    neededRotation.x = 0;
                    neededRotation.z = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.9f);
                    if (limite > 75)
                        fire();
                }

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
        if (move > 90)
        {
            _posicionLider = transform.position;
            _posicionLider.x += Random.Range(-20.0f, 20.0f);
            _posicionLider.z += Random.Range(-20.0f, 20.0f);
            move = 0;

        }
        transform.position = Vector3.MoveTowards(transform.position, _posicionLider, 2 * Time.deltaTime);

    }
    public void timeToHaulYoArses(int pos)
    {
        if (pos < 6)
        {
            ordenPos = pos;
            _estado = estados.bajoOrdenes;
        }
    }
}