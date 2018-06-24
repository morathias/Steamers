using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class protoScriptAC : Overlord
{
    public int Rango = 1;
    Quaternion neededRotation;
    private int commandTime = 0;
    private int commandInit;
    private bool onOrder = false;
    Vector3 _posicionLider;
    float move = 0;
    float limite = 0;
    public ParticleSystem shout;
    // public ParticleSystem _balaE;
    private List<GameObject> unidades;
    int count;
    Infantry troop;
    Shield sTroop;
    int numeroSoldado = 0;
    int numeroEscudo = 0;
    Animator _animations;
    Transform Target;
    int RangoBusqueda = 30;
    int RangoRush = 10;
    bool stayput = false;
    float cool = 0;
    int velocidad = 0;
    float random = 0;
    int random2;
    int cartucho = 0;
    float intervalo = 0.2f;
    float prox = 0.0f;
    bool range = true;
    int daño2 = 2;
    Vector3 specific;
    public UnityEngine.AI.NavMeshAgent navigator { get; private set; }
    private Rigidbody _rigidBody;

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
                    if (limite > 1)
                    {

                        if (Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
                        {
                            {
                                _estado = estados.normal;
                                fire();

                            }


                        }
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
    void formUp()
    {

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
        //_animations.Play("Armature|shoot");

        //_balaE.startLifetime = Rango / _balaE.startSpeed;

        //_balaE.transform.position = transform.position;
        //_balaE.Emit(1);
        //limite = 0;
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