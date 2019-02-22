using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class protoScriptAC : Overlord
{
    public int Rango = 10;
    float timeLeft = 0;
    float timeCap = 0;
    Transform capitanPos;
    GameObject Leader;
    Vector3 _posicionLider;
    float speed;
    private Shield sTroop;
    public ParticleSystem shout;
    private Infantry troop;
    public Collider _shieldCollider;
    private int numeroSoldado;
    private int numeroEscudo;
    public float shootTime;
    private List<GameObject> unidades;

    override protected void Start()
    {
        base.Start();
        _balaE = GetComponentInChildren<ParticleSystem>();
        _balaE.GetComponent<DañoBalas>().setDaño(daño);
        _animations = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        navigator.stoppingDistance = 0;
        navigator.speed = 0.8f;
        navigator.updateRotation = false;
        _pattern = Pattern.MOVING;
        unidades = new List<GameObject>();


    }
    override protected void Update()
    {
        base.Update();

        Debug.Log(_estado);
        switch (_estado)
        {
            case State.NORMAL:
                break;
            case State.ALARM:
                break;
            case State.AGGRESIVE:
                timeCap += timeLeft += Time.deltaTime * Time.timeScale;
                Debug.Log(_pattern);
                switch (_pattern)
                {

                    case Pattern.ATTACK:
                        //EjecutarAnimacionDeBashAttack
                        _pattern = Pattern.MOVING;
                        break;

                    case Pattern.MOVING:
                        Debug.Log("oving");
                        move();
                        //_rigidBody.velocity = navigator.desiredVelocity;

                        neededRotation = Quaternion.LookRotation(playerTf - transform.position);
                        neededRotation.x = 0;
                        neededRotation.z = 0;
                        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 2f);

                        if (Random.Range(1, 30) < 1 && playerStats.level>8)
                        {
                            timeCap = 0;
                            _pattern = Pattern.SPEEDBOOST;
                        }
                        if (timeLeft > 3 && Vector3.Distance(transform.position, playerTf) < 70)
                        {
                            transform.LookAt(playerTf);
                            RaycastHit ICU;

                            if (Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
                            {
                                _balaE.Emit(1);
                                timeLeft = 0;
                                move();

                            }

                        }
                        break;

                    case Pattern.AIMING:

                        break;

                    case Pattern.SPEEDBOOST:
                        Collider[] colliders = Physics.OverlapSphere(transform.position, 50);
                        shout.Emit(50);

                        foreach (Collider hit in colliders)
                        {
                            if (hit.gameObject.tag == "EnemyS" && numeroSoldado < 6)
                            {
                                troop = hit.gameObject.GetComponent<Infantry>();
                                Debug.Log("name hit" + hit.name);
                                if (troop.setPointMan(gameObject, numeroSoldado))
                                {
                                    unidades.Add(troop.gameObject);
                                    numeroSoldado++;
                                }

                            }

                            if (hit.gameObject.tag == "ArmoredE" && numeroEscudo < 2)
                            {
                                sTroop = hit.gameObject.GetComponent<Shield>();
                                if (sTroop.setPointMan(gameObject, numeroSoldado))
                                {
                                    unidades.Add(sTroop.gameObject);
                                    numeroEscudo++;
                                }
                            }

                        }
                        Debug.Log(unidades.Count);
                        if (unidades.Count > 2)
                        {
                            for (int i = 0; i < unidades.Count; i++)
                            {
                                unidades[i].GetComponent<Overlord>().setEvent(Events.capIsHere);

                            }
                            setEvent(Events.capIsOut);
                        }
                        else
                        {
                            for (int i = 0; i < unidades.Count; i++)
                            {
                                unidades[i].GetComponent<Overlord>().setEvent(Events.blunted);
                                

                            }

                            _pattern = Pattern.MOVING;
                        }
                        numeroSoldado = 0;
                        numeroEscudo = 0;
                        Debug.Log(_pattern);
                        break;
                }

                break;
            case State.FORMATION:
                timeCap += Time.deltaTime * Time.timeScale;
                move();
                neededRotation = Quaternion.LookRotation(playerTf - transform.position);
                neededRotation.x = 0;
                neededRotation.z = 0;

                transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 2.5f);

                if (timeCap > 2)
                {
                    RaycastHit ICU;

                    if (Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
                    {
                        timeCap = 0;
                        _balaE.Emit(1);
                        move();

                    }
                }
                checkAmout();
                break;

            default:
                break;
        }

    }
    private void checkAmout()
    {
        Debug.Log(unidades.Count + "unidade 2s");
        if (unidades.Count < 3)
        {
            for (int i = 0; i < unidades.Count; i++)
            {
                unidades[i].GetComponent<Overlord>().setEvent(Events.blunted);
            }
        }
    }

    override protected void disarm()
    {
        for (int i = 0; i < unidades.Count; i++)
        {
            unidades[i].GetComponent<Overlord>().setEvent(Events.blunted);
        }
    }

    public void removeFromList(GameObject unit)
    {
        unidades.Remove(unit);
    }
    private void move()
    {
        navigator.isStopped = false;
        navigator.destination = playerTf;

    }
    private bool reachedDestination()
    {
        if (!navigator.pathPending)
        {
            if (navigator.remainingDistance <= navigator.stoppingDistance)
            {
                if (!navigator.hasPath || navigator.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
//    public int Rango = 1;
//    Quaternion neededRotation;
//    private int commandTime = 0;
//    private int commandInit;
//    private bool onOrder = false;
//    Vector3 _posicionLider;
//    float move = 0;
//    float limite = 0;
//    public ParticleSystem shout;
//    // public ParticleSystem _balaE;
//    private List<GameObject> unidades;
//    int count;
//    Infantry troop;
//    Shield sTroop;
//    int numeroSoldado = 0;
//    int numeroEscudo = 0;
//    Animator _animations;
//    Transform Target;
//    int RangoBusqueda = 30;
//    int RangoRush = 10;
//    bool stayput = false;
//    float cool = 0;
//    int velocidad = 0;
//    float random = 0;
//    int random2;
//    int cartucho = 0;
//    float intervalo = 0.2f;
//    float prox = 0.0f;
//    bool range = true;
//    int daño2 = 2;
//    Vector3 specific;
//   // public UnityEngine.AI.NavMeshAgent navigator { get; private set; }
//    private Rigidbody _rigidBody;

//    override protected void Start()
//    {
//        base.Start();
//        unidades = new List<GameObject>();
//        //_stats.applyDamage(1);
//        Target = objective.transform;
//        fichador = objective.transform;
//        commandInit = Random.Range(120, 500);
//        _animations = GetComponent<Animator>();
//        _estado = estados.normal;
//    //    _balaE.GetComponent<DañoBalas>().setDaño(daño2);
//        _rigidBody = GetComponent<Rigidbody>();
//    }

//    void Update()
//    {

//        if (Vector3.Distance(transform.position, fichador.position) < 40)
//        {
//            switch (_estado)
//            {
//                case estados.normal:
//                    if (_stats.dead == true)
//                    {
//                        gameObject.SetActive(false);
//                      //  Time.timeScale = 0;
//                    }

//                    if (commandTime > commandInit)
//                    {
//                        commandTime = 0;
//                        _estado = estados.apuntando;
//                    }
//                    limite += Time.deltaTime * Time.timeScale;
//                    if (limite > 1.20)
//                        fire();
//                    commandTime++;
//                    _animations.Play("Armature|running");

//                    break;

//                case estados.bajoOrdenes:
//                    check();
//                    if (_stats.dead == true)
//                    {
//                        count = unidades.Count;
//                        for (int f = 0; f < count; f++)
//                        {
//                            unidades[f].GetComponent<Overlord>().fear();
//                            count = unidades.Count;
//                        }

//                        Destroy(gameObject);
//                        //Time.timeScale = 0;
//                    }
//                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
//                    neededRotation.x = 0;
//                    neededRotation.z = 0;

//                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.75f);
//                    _animations.Play("Armature|running");

//                    limite += Time.deltaTime * Time.timeScale;
//                    if (limite > 1.5f)
//                    {
//                      //  _estado = estados.rage;
//                    }


//                    break;
//                case estados.rage:
//                    if (_stats.dead == true)
//                        Destroy(gameObject);
//                    RaycastHit ICU;
//                    transform.LookAt(fichador);

//                    limite += Time.deltaTime * Time.timeScale;
//                    if (limite > 1)
//                    {

//                        if (Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
//                        {
//                            {
//                                _estado = estados.normal;
//                                fire();

//                            }


//                        }
//                    }

//                    break;
//                case estados.apuntando:
//                    _animations.Play("Armature|shouting");
//                    Vector3 explosionPos = transform.position;
//                    Collider[] colliders = Physics.OverlapSphere(explosionPos, 50);
//                    shout.Emit(50);

//                    foreach (Collider hit in colliders)
//                    {
//                        if (hit.gameObject.tag == "EnemyS")
//                        {
//                            troop = hit.gameObject.GetComponent<Infantry>();

//                            if (troop.begin(numeroSoldado, this.gameObject))
//                            {
//                                unidades.Add(troop.gameObject);
//                                numeroSoldado++;
//                            }
//                        }

//                        if (hit.gameObject.tag == "ArmoredE")
//                        {
//                            sTroop = hit.gameObject.GetComponent<Shield>();

//                            if (sTroop.begin(numeroEscudo, this.gameObject))
//                            {
//                                unidades.Add(sTroop.gameObject);
//                                numeroEscudo++;
//                            }
//                        }
//                    }

//                    onOrder = true;
//                    if (unidades.Count > 3)
//                    {
//                        check();
//                    }
//                    _estado = estados.bajoOrdenes;

//                    break;
//            }
//        }
//    }
//    void formUp()
//    {

//    }

//    void fire()
//    {
//        //_animations.Play("Armature|shoot");

//        //_balaE.startLifetime = Rango / _balaE.startSpeed;

//        //_balaE.transform.position = transform.position;
//        //_balaE.Emit(1);
//        //limite = 0;
//    }

//    void check()
//    {
//        Debug.Log("UCHEING");
//        count = unidades.Count;
//        if (count < 3)
//        {
//            for (int f = 0; f < count; f++)
//            {
//                unidades[f].GetComponent<Overlord>().reset();
//                unidades.RemoveAt(f);
//                count = unidades.Count;

//                onOrder = false;
//            }
//            unidades[0].GetComponent<Overlord>().reset();
//            unidades.RemoveAt(0);
//            numeroSoldado = 0;
//            numeroEscudo = 0;
//            count = 0;
//            _estado = estados.normal;
//        }

//        else for (int i = 0; i < unidades.Count; i++)
//            {
//                EnemyHealth lookAtIt = unidades[i].GetComponent<EnemyHealth>();
//                Debug.Log("Unit: " + unidades[i] + " = " + unidades[i].GetComponent<EnemyHealth>().isDead());
//                if (lookAtIt.isDead() == true)
//                {
//                    Destroy(unidades[i]);
//                    unidades.RemoveAt(i);
//                    count = unidades.Count;
//                }
//            }
//    }
//}

