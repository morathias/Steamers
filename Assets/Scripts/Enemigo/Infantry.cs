using UnityEngine;
using System.Collections;

public class Infantry : Overlord
{

    public int Rango = 10;
    float timeLeft = 0;
    Transform capitanPos;
    GameObject Leader;
    Vector3 _posicionLider;
    Animator _animations;
    

    override protected void Start()
    {
        base.Start();
        _balaE = GetComponentInChildren<ParticleSystem>();
        _balaE.GetComponent<DañoBalas>().setDaño(daño);
        _animations = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        navigator.stoppingDistance = 0;
        _pattern = Pattern.MOVING;
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
                    case Pattern.AIMING:
                        RaycastHit ICU;
                        //animator.SetTrigger("aim"); inciar animacion de apuntado
                        neededRotation = Quaternion.LookRotation(playerTf - transform.position);
                        neededRotation.x = 0;
                        neededRotation.z = 0;

                        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 2.5f);

                        if (timeLeft < 1.5)
                        {
                            if (Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
                            {
                                //animator.SetTrigger("Attack"); iniciar animacion de ataque
                                _pattern = Pattern.ATTACK;

                            }
                        }
                        else

                            move();
                        break;

                    case Pattern.ATTACK:
                        _balaE.Emit(1);
                        timeLeft = 0;
                        move();
                        break;

                    case Pattern.MOVING:
                        _rigidBody.velocity = navigator.desiredVelocity;
                        if (timeLeft > 3 && Vector3.Distance(transform.position, playerTf) < 70)
                        {
                            navigator.isStopped = true;
                            timeLeft = 0;
                            // animator.SetBool("Running", false);
                            _pattern = Pattern.AIMING;
                        }
                        else if (reachedDestination())
                            move();
                        break;

                    case Pattern.SPEEDBOOST:
                        // animator.SetBool("dodge", true); inicia animacino de esquivado, la animacion no debe ser mas de 0.8 seg.
                        transform.Translate(Vector3.forward * 15 * Time.deltaTime);
                        if (timeLeft > 0.8)
                        {
                            _pattern = Pattern.AIMING;
                            timeLeft = 0;
                        }
                        break;
                    default:
                        break;
                }
                break;
            case State.DEAD:
                break;
            case State.STUNNED:
                break;
            case State.FORMATION:
                break;

            default:
                break;
        }

    }

    private void move()
    {
        _pattern = Pattern.MOVING;
        if (Random.Range(1, 10)>5)
        {
            navigator.isStopped = false;
            Vector3 newPos = RandomNavSphere(transform.position, 3, -1);
            navigator.SetDestination(newPos);
        }
        else
        {
            _pattern = Pattern.SPEEDBOOST; // Dodge, logica que spamee adrede para ver, si no queres que lo haga, comenta solo esto!!
        }


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
//using UnityEngine;
//using System.Collections;

//public class Infantry : Overlord
//{

//    private Rigidbody _rigidBody;
//    public int Rango = 10;
//    float timeLeft = 4;
//    float limite = 0;
//    float move = 0;
//    Transform capitanPos;
//    public ParticleSystem _balaE;
//    Quaternion neededRotation;
//    GameObject Leader;
//    Vector3 _posicionLider;
//    Animator _animations;
//    public bool isBoss = false;

//    override protected void Start()
//    {
//        base.Start();
//        _balaE.GetComponent<DañoBalas>().setDaño(daño);
//        _animations = GetComponent<Animator>();
//        _rigidBody = GetComponent<Rigidbody>();
//    }
//    void Update()
//    {
//        switch (_estado)
//        {
//            case estados.normal:

//                if (_stats.dead == true)
//                {
//                    countdown--;
//                    Destroy(gameObject);
//                }
//                if (Vector3.Distance(transform.position, fichador.position) < 30)
//                {
//                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
//                    neededRotation.x = 0;
//                    neededRotation.z = 0;

//                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 5.0f);
//                    if (limite > 2)
//                    {
//                        playerTF.x = playerTF.x + Random.Range(-10, 10.0f);
//                        playerTF.z = playerTF.z + Random.Range(-10, 10.0f);
//                        limite = 0;
//                        _estado = estados.rage;
//                    }
//                    if (Vector3.Distance(transform.position, fichador.position) > 8)
//                    {
//                      //  moveIt();
//                        _animations.Play("Armature|running");
//                    }

//                    limite += Time.deltaTime * Time.timeScale;
//                }
//                else
//                    _animations.Play("Armature|iddle_001");
//                break;

//            case estados.rage:
//                if (_stats.dead == true)
//                    Destroy(gameObject);
//                RaycastHit ICU;
//                transform.LookAt(fichador);
//                if (Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
//                {
//                    limite += Time.deltaTime * Time.timeScale;
//                    if (limite > 1)
//                    {

//                        {
//                            _estado = estados.normal;
//                            fire();
//                            _animations.Play("Armature|shoot");
//                        }


//                    }
//                    else
//                        _animations.Play("Armature|apuntando");
//                }
//                else
//                    _estado = estados.normal;

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
//                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 3);
//                        break;
//                    case 1:
//                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 3) - (capitanPos.right * 3);

//                        break;
//                    case 2:
//                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 3) - (capitanPos.right * -3);

//                        break;
//                    case 3:
//                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 6);

//                        break;
//                    case 4:
//                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 6) - (capitanPos.right * 3);

//                        break;
//                    case 5:
//                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 6) - (capitanPos.right * -3);
//                        break;

//                }
//               // navigator.SetDestination(_posicionLider);
//              //  navigator.transform.position = transform.position;

//                _animations.Play("Armature|running");
//             //   _rigidBody.velocity = navigator.desiredVelocity;
//                if (Vector3.Distance(transform.position, fichador.position) < 30)
//                {
//                    limite += Time.deltaTime * Time.timeScale; ;
//                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
//                    neededRotation.x = 0;
//                    neededRotation.z = 0;
//                    transform.LookAt(fichador);
//                    if (limite > 1.5)
//                    {
//                        fire();
//                        _animations.Play("Armature|shoot");
//                    }

//                }

//                break;
//        }


//    }

//    void fire()
//    {
//        _balaE.startLifetime = Rango / _balaE.startSpeed;

//        _balaE.transform.position = transform.position;
//        _balaE.Emit(1);

//        limite = 0;
//    }


//    void moveIt(float chaos)
//    {
//        neededRotation = Quaternion.LookRotation(fichador.position - transform.position);
//        neededRotation *= Quaternion.Euler(0, Random.Range(90.0f, 270.0f), 0);
//        neededRotation.x = 0;
//        neededRotation.z = 0;
//        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 5f);

//        playerTF.x = playerTF.x + Random.Range(10, 20.0f)* Random.Range(-1,1);
//        playerTF.z = playerTF.z + Random.Range(10, 20.0f) * Random.Range(-1, 1);
//        navigator.SetDestination(playerTF);
//        navigator.transform.position = transform.position;
//        _rigidBody.velocity = navigator.desiredVelocity;
//    }
//    public bool begin(int pos, GameObject PointMan)
//    {
//        if (pos < 6 && _estado != estados.bajoOrdenes)
//        {
//            ordenPos = pos;
//            _estado = estados.bajoOrdenes;
//            Leader = PointMan;
//            capitanPos = Leader.transform;
//            return true;
//        }
//        else
//            return false;

//    }


//    public void MoveIt(Vector3 velocity, Vector3 lookAt)
//    {

//        _rigidBody.velocity = velocity;
//    }
//}