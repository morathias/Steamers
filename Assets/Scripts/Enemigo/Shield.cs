using UnityEngine;
using System.Collections;

public class Shield : Overlord
{
    public int Rango = 10;
    float timeLeft = 0;
    float speed;

    public Collider _shieldCollider;

    override protected void Start()
    {
        base.Start();
        _animations = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        navigator.stoppingDistance = 0;
        navigator.speed = 0.8f;
        _pattern = Pattern.MOVING;
    }
    override protected void disarm()
    {
        pointMan.gameObject.GetComponent<protoScriptAC>().removeFromList(this.gameObject);
    }

    public void Attack()
    {
        _animations.SetTrigger("Attack");
        _pattern = Pattern.MOVING;
        setEvent(Events.blunted);
    }

    override protected void Update()
    {
        base.Update();

        switch (_estado)
        {
            case State.NORMAL:
                break;
            case State.ALARM:
                break;
            case State.AGGRESIVE:
                timeLeft += Time.deltaTime * Time.timeScale;
                switch (_pattern)
                {

                    case Pattern.ATTACK:
                        //EjecutarAnimacionDeBashAttack
                        _pattern = Pattern.MOVING;
                        break;

                    case Pattern.MOVING:
                        _rigidBody.velocity = navigator.desiredVelocity;
                        move();
                        if (timeLeft > 5 && Vector3.Distance(transform.position, playerTf) < 70 && playerStats.level > 8)
                        {
                            navigator.isStopped = true;
                            timeLeft = 0;
                            _animations.SetBool("Range", false); // Detieene animacion de corrida
                            _pattern = Pattern.AIMING;
                        }
                        //  else if (reachedDestination())
                        break;

                    case Pattern.AIMING:

                        transform.LookAt(playerTf);
                        RaycastHit ICU;

                        if (Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
                        {
                            _animations.SetBool("Range", true);
                            _animations.speed *= 1.5f;
                            _pattern = Pattern.SPEEDBOOST;
                        }
                        break;

                    case Pattern.SPEEDBOOST:

                        transform.Translate(Vector3.forward * speed * Time.deltaTime);
                        speed += 0.5f;
                        if (timeLeft > 1)
                        {
                            _animations.speed -= 0.5f;
                            speed = 0;
                            timeLeft = 0;
                            _animations.SetBool("Range", false);
                            _pattern = Pattern.MOVING;
                            stateMachine.setEvent((int)Events.blunted);
                        }
                        break;
                }
                break;
            case State.DEAD:
                break;
            case State.STUNNED:

                timeLeft += Time.deltaTime * Time.timeScale;
                if (timeLeft > 1f)
                {
                    _animations.ResetTrigger("Attack");
                    _animations.SetBool("Range", true);
                    timeLeft = 0;
                    stateMachine.setEvent((int)Events.recover);

                }
                break;
            case State.FORMATION:
                transform.position = setPosition(positionFormation, pointMan.transform);

                neededRotation = Quaternion.LookRotation(playerTf - transform.position);
                neededRotation.x = 0;
                neededRotation.z = 0;

                transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 3f);

                transform.position = setPosition(positionFormation, pointMan.transform);
                break;

            default:
                break;
        }

    }

    private void move()
    {
        _animations.SetBool("Range", true);
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

    public Vector3 setPosition(int pos, Transform pointMan)
    {
        if (positionFormation > 0)
            return new Vector3(pointMan.position.x, pointMan.position.y, pointMan.position.z) - (pointMan.right * 3);
        else
            return new Vector3(pointMan.position.x, pointMan.position.y, pointMan.position.z) - (pointMan.right * -3);

    }
    override public bool setPointMan(GameObject PM, int order)
    {
        if (order < 2 && _estado != State.FORMATION)
        {
            positionFormation = order;
            pointMan = PM;
            return true;

        }
        else
            return false;

    }
}

//    //public UnityEngine.AI.NavMeshAgent navigator { get; private set; }
//    private Rigidbody _rigidBody;
//    public int Rango = 1;
//    // float intervalo = 0.2f;
//    public int velocidad = 2;
//    Transform capitanPos;
//    Quaternion neededRotation;
//    GameObject Leader;
//    Infantry troop;
//    int damage = 5;
//    bool stayput = true;
//    float timer = 0;
//    float move = 0;
//    float timeLeft = 4;
//   // public Collider shield;
//    Vector3 _posicionLider;
//    Animator _animations;
//    public bool protect = false;

//    override protected void Start()
//    {
//        base.Start();
//        _stats.applyDamage(1);

//        fichador = objective.transform;
//        _animations = GetComponent<Animator>();
//        _rigidBody = GetComponent<Rigidbody>();


//    }
//    void Update()
//    {
//        playerTF = fichador.position;
//        switch (_estado)
//        {
//            case estados.protect:
//                {
//                    if (troop.GetComponent<EnemyHealth>().dead == true)
//                    {
//                        Destroy(troop.gameObject);
//                        _estado = estados.normal;
//                    }
//                    if (_stats.dead == true)
//                    {
//                        troop.reset();
//                        Destroy(gameObject);
//                    }

//                    if (Vector3.Distance(transform.position, fichador.position) < 30)
//                    {
//                        _animations.SetBool("Alert", true);
//                        neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
//                        neededRotation.x = 0;
//                        neededRotation.z = 0;

//                        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.8f);
//                        if (Vector3.Distance(transform.position, fichador.position) > 8)
//                   //         moveIt();
//                        move += Time.deltaTime * Time.timeScale;
//                        stayput = false;
//                    }
//                    else if (!stayput)
//                    {

//                        _animations.SetBool("Range", false);
//                    }


//                }
//                break;

//            case estados.normal:

//                if (_stats.dead == true)
//                    Destroy(gameObject);

//                if (Vector3.Distance(transform.position, fichador.position) < 30)
//                {

//                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
//                    neededRotation.x = 0;
//                    neededRotation.z = 0;
//                    //if (protect)
//                    //{
//                    //    searchPro();
//                    //}
//                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 1f);
//                    if (!stayput)
//                        _animations.SetBool("Range", true);
//                    else
//                    {
//                        stayput = false;
//                        _animations.Play("ready");
//                    }
//                    if (Vector3.Distance(transform.position, fichador.position) > 8)
//                    {
//                       // moveIt();
//                    }

//                }
//                else if (!stayput)
//                {

//                    _animations.SetBool("Range", false);
//                }
//                timer += Time.deltaTime * Time.timeScale; ;

//                if (timer > 2 && protect)
//                {
//                    timer = 0;
//                    Debug.Log("Bolo");
//                    searchPro();
//                }

//                break;

//            case estados.fear:
//                if (_stats.dead == true)
//                    Destroy(gameObject);
//                moveIt(Random.Range(90.0f, 270.0f));

//                timeLeft -= Time.deltaTime * Time.timeScale;
//                if (timeLeft < 0)
//                {
//                    timeLeft = 4;
//                    _estado = estados.normal;
//                }

//                break;

//            case estados.bajoOrdenes:

//                switch (ordenPos)
//                {
//                    case 0:
//                        _posicionLider = new Vector3(PointMan.position.x, PointMan.position.y, PointMan.position.z) - (capitanPos.right * 3);
//                        break;
//                    case 1:
//                        _posicionLider = new Vector3(PointMan.position.x, PointMan.position.y, PointMan.position.z) - (capitanPos.right * -3);

//                        break;

//                    default:
//                        _estado = estados.normal;
//                        break;

//                }
//                navigator.SetDestination(_posicionLider);
//                navigator.transform.position = transform.position;

//                _animations.Play("Walking");
//                transform.position = Vector3.MoveTowards(transform.position, _posicionLider, 5 * Time.deltaTime);
//                transform.rotation = capitanPos.rotation;
//                break;
//        }

//    }


//    public bool begin(int pos, GameObject PointMan)
//    {
//        if (pos < 2 && _estado != estados.bajoOrdenes)
//        {
//            ordenPos = pos;
//            _estado = estados.bajoOrdenes;
//            Leader = PointMan;
//            capitanPos = Leader.transform;
//            return true;
//        }
//        else
//        {

//            return false;
//        }
//    }
//    //void moveIt()
//    //{
//    //    move += Time.deltaTime * Time.timeScale;
//    //    if (move > 5)
//    //    {
//    //        _posicionLider = transform.position;
//    //        _posicionLider.x += Random.Range(-20.0f, 20.0f);
//    //        _posicionLider.z += Random.Range(-20.0f, 20.0f);
//    //        move = 0;

//    //    }
//    //    if (!stayput)
//    //        _animations.SetBool("Range", true);
//    //    else
//    //    {
//    //        stayput = false;
//    //        _animations.Play("ready");
//    //    }

//    //    navigator.SetDestination(playerTF);
//    //    navigator.transform.position = transform.position;
//    //    _rigidBody.velocity = navigator.desiredVelocity * 0.8f;
//    //}

//    void searchPro()
//    {

//        _animations.SetBool("Range", false);

//        Vector3 explosionPos = transform.position;
//        Collider[] colliders = Physics.OverlapSphere(explosionPos, 5);

//        foreach (Collider hit in colliders)
//        {
//            if (hit.gameObject.tag == "EnemyS")
//            {
//                troop = hit.gameObject.GetComponent<Infantry>();
//                if (troop._estado != estados.bajoOrdenes)
//                {
//                    troop.begin(0, this.gameObject);
//                    Debug.Log("yo " + gameObject + "lacge");
//                    _estado = estados.protect;
//                    Debug.Log(troop.status());
//                    break;
//                }

//            }

//        }

//    }


//    void moveIt(float chaos)
//    {
//        neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
//        neededRotation *= Quaternion.Euler(0, Random.Range(90.0f, 270.0f), 0);
//        neededRotation.x = 0;
//        neededRotation.z = 0;
//        _animations.SetBool("Range", true);
//        transform.Translate(Vector3.forward * 8 * Time.deltaTime);

//    }
//}
