using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class protoScriptAC : Overlord
{
    Animator _animations;
    Transform Target;
    public int Rango = 1;

    Vector3 _posicionLider;
    float move = 0;
    float limite = 0;
    public ParticleSystem shout;

    private List<GameObject> unidades;
    Infantry troop;
    Shield sTroop;
    int numeroSoldado = 0;
    int numeroEscudo = 0;
    int commandTime = 0;
    int commandInit;
    bool onOrder = false;

    int count;

    int RangoBusqueda = 30;
    int RangoRush = 10;
    int cartucho = 0;
    float intervalo = 0.2f;
    float prox = 0.0f;
    int daño2 = 2;

    override protected void Start()
    {
        base.Start();
        unidades = new List<GameObject>();
        //_stats.applyDamage(1);
        Target = objective.transform;
        fichador = objective.transform;
        commandInit = Random.Range(120, 500);
        _animations = GetComponent<Animator>();
        _estado = estados.normal;
        //    _balaE.GetComponent<DañoBalas>().setDaño(daño2);
        _rigidBody = GetComponent<Rigidbody>();
        navigator = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        fichadorPos = fichador.position;

        Debug.Log("unidades " + count);
        if (Vector3.Distance(transform.position, fichador.position) < 40)
        {
            switch (_estado)
            {
                case estados.normal:
                    if (_stats.dead == true)
                    {
                        gameObject.SetActive(false);
                        //  Time.timeScale = 0;
                    }

                    if (commandTime > commandInit)
                    {
                        commandTime = 0;
                        _estado = estados.apuntando;
                    }
                    limite += Time.deltaTime * Time.timeScale;
                    if (limite > 1.20)
                        fire();
                    commandTime++;
                    _animations.Play("Armature|running");
                    moveIt();
                    break;

                case estados.bajoOrdenes:
                    check();
                    if (_stats.dead == true)
                    {
                        count = unidades.Count;
                        for (int f = 0; f < count; f++)
                        {
                            unidades[f].GetComponent<Overlord>().fear();
                            count = unidades.Count;
                        }

                        Destroy(gameObject);
                        //Time.timeScale = 0;
                    }
                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                    neededRotation.x = 0;
                    neededRotation.z = 0;

                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.75f);
                    _animations.Play("Armature|running");
                    if (Vector3.Distance(transform.position, fichador.position) > 12)
                        moveIt();

                    limite += Time.deltaTime * Time.timeScale;
                    if (limite > 1.5f)
                    {
                        //  _estado = estados.rage;
                    }


                    break;
                case estados.rage:
                    if (_stats.dead == true)
                        Destroy(gameObject);

                    RaycastHit ICU;
                    transform.LookAt(fichador);
                    limite += Time.deltaTime * Time.timeScale;
                    if (limite > 1 && Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
                    {
                        _estado = estados.normal;
                        fire();
                    }

                    break;

                case estados.apuntando:
                    _animations.Play("Armature|shouting");
                    Vector3 explosionPos = transform.position;
                    Collider[] colliders = Physics.OverlapSphere(explosionPos, 50);
                    shout.Emit(50);

                    foreach (Collider hit in colliders)
                    {
                        if (hit.gameObject.tag == "EnemyS")
                        {
                            troop = hit.gameObject.GetComponent<Infantry>();

                            if (troop.begin(numeroSoldado, this.gameObject))
                            {
                                unidades.Add(troop.gameObject);
                                numeroSoldado++;
                            }
                        }

                        if (hit.gameObject.tag == "ArmoredE")
                        {
                            sTroop = hit.gameObject.GetComponent<Shield>();

                            if (sTroop.begin(numeroEscudo, this.gameObject))
                            {
                                unidades.Add(sTroop.gameObject);
                                numeroEscudo++;
                            }
                        }
                    }

                    onOrder = true;
                    if (unidades.Count > 3)
                    {
                        check();
                    }
                    _estado = estados.bajoOrdenes;

                    break;
            }
        }
    }

    void moveIt()
    {
        navigator.SetDestination(fichadorPos);
        navigator.transform.position = transform.position;
        _rigidBody.velocity = navigator.desiredVelocity * 0.3f;
        _animations.Play("Armature|running");
    }

    void fire()
    {
        //    if (Time.time >= prox && cartucho < 20)
        //    {

        //        prox = Time.time + intervalo;
        //        _balaE.startLifetime = Rango / _balaE.startSpeed;
        //        //_balaE.transform.position = transform.position;
        //        _balaE.transform.LookAt(objective.transform);

        //_animations.Play("Armature|shoot");
        //        _balaE.Emit(1);
        //        cartucho++;
        //        Debug.Log(cartucho);
        //    }

        //    if (cartucho >= 20)
        //        _estado = estados.recarga;

    }

    void check()
    {
        Debug.Log("UCHEING");
        count = unidades.Count;
        if (count < 3)
        {
            for (int f = 0; f < count; f++)
            {
                unidades[f].GetComponent<Overlord>().reset();
                unidades.RemoveAt(f);
                count = unidades.Count;

                onOrder = false;
            }
            unidades[0].GetComponent<Overlord>().reset();
            unidades.RemoveAt(0);
            numeroSoldado = 0;
            numeroEscudo = 0;
            count = 0;
            _estado = estados.normal;
        }

        else for (int i = 0; i < unidades.Count; i++)
            {
                EnemyHealth lookAtIt = unidades[i].GetComponent<EnemyHealth>();
                Debug.Log("Unit: " + unidades[i] + " = " + unidades[i].GetComponent<EnemyHealth>().isDead());
                if (lookAtIt.isDead() == true)
                {
                    Destroy(unidades[i]);
                    unidades.RemoveAt(i);
                    count = unidades.Count;
                }
            }
    }
}


