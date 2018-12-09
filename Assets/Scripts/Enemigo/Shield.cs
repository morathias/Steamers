using UnityEngine;
using System.Collections;

public class Shield : Overlord
{
    //public UnityEngine.AI.NavMeshAgent navigator { get; private set; }
    private Rigidbody _rigidBody;
    public int Rango = 1;
    // float intervalo = 0.2f;
    public int velocidad = 2;
    Transform capitanPos;
    Quaternion neededRotation;
    GameObject Leader;
    Infantry troop;
    int damage = 5;
    bool stayput = true;
    float timer = 0;
    float move = 0;
    float timeLeft = 4;
   // public Collider shield;
    Vector3 _posicionLider;
    Animator _animations;
    public bool protect = false;

    override protected void Start()
    {
        base.Start();
        _stats.applyDamage(1);

        fichador = objective.transform;
        _animations = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();


    }
    void Update()
    {
        playerTF = fichador.position;
        switch (_estado)
        {
            case estados.protect:
                {
                    if (troop.GetComponent<EnemyHealth>().dead == true)
                    {
                        Destroy(troop.gameObject);
                        _estado = estados.normal;
                    }
                    if (_stats.dead == true)
                    {
                        troop.reset();
                        Destroy(gameObject);
                    }

                    if (Vector3.Distance(transform.position, fichador.position) < 30)
                    {
                        _animations.SetBool("Alert", true);
                        neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                        neededRotation.x = 0;
                        neededRotation.z = 0;

                        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.8f);
                        if (Vector3.Distance(transform.position, fichador.position) > 8)
                            moveIt();
                        move += Time.deltaTime * Time.timeScale;
                        stayput = false;
                    }
                    else if (!stayput)
                    {

                        _animations.SetBool("Range", false);
                    }


                }
                break;

            case estados.normal:

                if (_stats.dead == true)
                    Destroy(gameObject);

                if (Vector3.Distance(transform.position, fichador.position) < 30)
                {

                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                    neededRotation.x = 0;
                    neededRotation.z = 0;
                    //if (protect)
                    //{
                    //    searchPro();
                    //}
                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 1f);
                    if (!stayput)
                        _animations.SetBool("Range", true);
                    else
                    {
                        stayput = false;
                        _animations.Play("ready");
                    }
                    if (Vector3.Distance(transform.position, fichador.position) > 8)
                    {
                        moveIt();
                    }

                }
                else if (!stayput)
                {

                    _animations.SetBool("Range", false);
                }
                timer += Time.deltaTime * Time.timeScale; ;

                if (timer > 2 && protect)
                {
                    timer = 0;
                    Debug.Log("Bolo");
                    searchPro();
                }

                break;

            case estados.fear:
                if (_stats.dead == true)
                    Destroy(gameObject);
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
                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.right * 3);
                        break;
                    case 1:
                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.right * -3);

                        break;

                    default:
                        _estado = estados.normal;
                        break;

                }
                navigator.SetDestination(_posicionLider);
                navigator.transform.position = transform.position;

                _animations.Play("Walking");
                transform.position = Vector3.MoveTowards(transform.position, _posicionLider, 5 * Time.deltaTime);
                transform.rotation = capitanPos.rotation;
                break;
        }

    }


    public bool begin(int pos, GameObject PointMan)
    {
        if (pos < 2 && _estado != estados.bajoOrdenes)
        {
            ordenPos = pos;
            _estado = estados.bajoOrdenes;
            Leader = PointMan;
            capitanPos = Leader.transform;
            return true;
        }
        else
        {
            
            return false;
        }
    }
    void moveIt()
    {
        move += Time.deltaTime * Time.timeScale;
        if (move > 5)
        {
            _posicionLider = transform.position;
            _posicionLider.x += Random.Range(-20.0f, 20.0f);
            _posicionLider.z += Random.Range(-20.0f, 20.0f);
            move = 0;

        }
        if (!stayput)
            _animations.SetBool("Range", true);
        else
        {
            stayput = false;
            _animations.Play("ready");
        }

        navigator.SetDestination(playerTF);
        navigator.transform.position = transform.position;
        _rigidBody.velocity = navigator.desiredVelocity * 0.8f;
    }

    void searchPro()
    {

        _animations.SetBool("Range", false);

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 5);

        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.tag == "EnemyS")
            {
                troop = hit.gameObject.GetComponent<Infantry>();
                if (troop._estado != estados.bajoOrdenes)
                {
                    troop.begin(0, this.gameObject);
                    Debug.Log("yo " + gameObject + "lacge");
                    _estado = estados.protect;
                    Debug.Log(troop.status());
                    break;
                }
               
            }

        }

    }


    void moveIt(float chaos)
    {
        neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
        neededRotation *= Quaternion.Euler(0, Random.Range(90.0f, 270.0f), 0);
        neededRotation.x = 0;
        neededRotation.z = 0;
        _animations.SetBool("Range", true);
        transform.Translate(Vector3.forward * 8 * Time.deltaTime);

    }
}
