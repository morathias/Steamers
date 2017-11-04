using UnityEngine;
using System.Collections;

public class Infantry : Overlord
{
    Transform fichador;
    public int Rango = 1;
    float timeLeft = 4;
    // float intervalo = 0.2f;
    //float prox = 0.0f;
    float limite = 0;
    // int cool = 0;
    float move = 0;
    Transform capitanPos;
    public ParticleSystem _balaE;
    Quaternion neededRotation;
    //  private int ordenPos;
    GameObject Leader;
    Vector3 _posicionLider;
    Animator _animations;


    override protected void Start()
    {
        base.Start();
        _balaE.GetComponent<DañoBalas>().setDaño(daño);
        fichador = objective.transform;
        _animations = GetComponent<Animator>();
    }
    void Update()
    {
   
        switch (_estado)
        {
            case estados.normal:
                
                if (dead == true)
                { 
                    Destroy(gameObject);
                }
                if (Vector3.Distance(transform.position, fichador.position) < Rango)
                {
                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                    neededRotation.x = 0;
                    neededRotation.z = 0;

                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 2.0f);
                    if (limite > 120)
                    {
                        limite = 0;
                        _estado = estados.rage;
                    }
                    moveIt();
                    _animations.Play("Armature|running");
                    limite++;
                }
                else
                    _animations.Play("Armature|iddle_001");


                break;

            case estados.rage:
                RaycastHit ICU;
                transform.LookAt(fichador);
             
                limite += Time.deltaTime * Time.timeScale;
                if (limite > 1)
                {

                    if (Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
                    {
                        {
                            _estado = estados.normal;
                            fire();
                            _animations.Play("Armature|shoot");
                        }


                    }
                }
                else
                    _animations.Play("Armature|apuntando");

                break;

            case estados.fear:
                moveIt(Random.Range(90.0f, 270.0f));

                timeLeft -= Time.deltaTime * Time.timeScale;
                if (timeLeft < 0)
                {
                    timeLeft = 4;
                    _estado = estados.normal;
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
                    limite += Time.deltaTime * Time.timeScale; ;
                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                    neededRotation.x = 0;
                    neededRotation.z = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.9f);
                    if (limite > 1.2)
                        fire();
                }

                break;
        }


    }

    void fire()
    {
        _balaE.startLifetime = Rango / _balaE.startSpeed;

        _balaE.transform.position = transform.position;
        _balaE.Emit(1);

        limite = 0; 
    }

    void moveIt()
    {
        move+= Time.deltaTime * Time.timeScale;
        if (move > 1.5f)
        {
            _posicionLider = transform.position;
            _posicionLider.x += Random.Range(-20.0f, 20.0f);
            _posicionLider.z += Random.Range(-20.0f, 20.0f);
            move = 0;

        }
        transform.position = Vector3.MoveTowards(transform.position, _posicionLider, 2 * Time.deltaTime);

    }

    void moveIt(float chaos)
    {
        neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
        neededRotation *= Quaternion.Euler(0, Random.Range(90.0f, 270.0f), 0);
        neededRotation.x = 0;
        neededRotation.z = 0;

        transform.Translate(Vector3.forward * 8 * Time.deltaTime);

    }
    public bool begin(int pos, GameObject PointMan)
    {
        if (pos < 6 && _estado != estados.bajoOrdenes)
        {
            ordenPos = pos;
            _estado = estados.bajoOrdenes;
            Leader = PointMan;
            capitanPos = Leader.transform;
            return true;
        }
        else
            return false;
    }
}