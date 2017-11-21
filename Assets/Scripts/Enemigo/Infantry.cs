using UnityEngine;
using System.Collections;

public class Infantry : Overlord
{
    public UnityEngine.AI.NavMeshAgent navigator { get; private set; }
    private Rigidbody _rigidBody;
    public int Rango = 10;
    float timeLeft = 4;
    float limite = 0;
    float move = 0;
    Transform capitanPos;
    public ParticleSystem _balaE;
    Quaternion neededRotation;
    GameObject Leader;
    Vector3 _posicionLider;
    Animator _animations;
    public bool isBoss = false;

    override protected void Start()
    {
        base.Start();
        _balaE.GetComponent<DañoBalas>().setDaño(daño);
        _animations = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        navigator = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
    }
    void Update()
    {
        fichadorPos = fichador.position;
        switch (_estado)
        {
            case estados.normal:

                if (_stats.dead == true)
                {
                    countdown--;
                    Destroy(gameObject);
                }
                if (Vector3.Distance(transform.position, fichador.position) < 30)
                {
                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                    neededRotation.x = 0;
                    neededRotation.z = 0;

                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 5.0f);
                    if (limite > 2)
                    {
                        fichadorPos.x = fichadorPos.x + Random.Range(-10, 10.0f);
                        fichadorPos.z = fichadorPos.z + Random.Range(-10, 10.0f);
                        limite = 0;
                        _estado = estados.rage;
                    }
                    if (Vector3.Distance(transform.position, fichador.position) > 8)
                    {
                        moveIt();
                        _animations.Play("Armature|running");
                    }

                    limite += Time.deltaTime * Time.timeScale;
                }
                else
                    _animations.Play("Armature|iddle_001");
                break;

            case estados.rage:
                if (_stats.dead == true)
                    Destroy(gameObject);
                RaycastHit ICU;
                transform.LookAt(fichador);
                if (Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
                {
                    limite += Time.deltaTime * Time.timeScale;
                    if (limite > 1)
                    {

                        {
                            _estado = estados.normal;
                            fire();
                            _animations.Play("Armature|shoot");
                        }


                    }
                    else
                        _animations.Play("Armature|apuntando");
                }
                else
                    _estado = estados.normal;

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
                navigator.SetDestination(_posicionLider);
                navigator.transform.position = transform.position;

                _animations.Play("Armature|running");
                _rigidBody.velocity = navigator.desiredVelocity;
                if (Vector3.Distance(transform.position, fichador.position) < 30)
                {
                    limite += Time.deltaTime * Time.timeScale; ;
                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                    neededRotation.x = 0;
                    neededRotation.z = 0;
                    transform.LookAt(fichador);
                    if (limite > 1.5)
                    {
                        fire();
                        _animations.Play("Armature|shoot");
                    }
                        
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
        navigator.SetDestination(fichadorPos);
        navigator.transform.position = transform.position;
        _rigidBody.velocity = navigator.desiredVelocity;

    }

    void moveIt(float chaos)
    {
        neededRotation = Quaternion.LookRotation(fichador.position - transform.position);
        neededRotation *= Quaternion.Euler(0, Random.Range(90.0f, 270.0f), 0);
        neededRotation.x = 0;
        neededRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 5f);

        fichadorPos.x = fichadorPos.x + Random.Range(10, 20.0f)* Random.Range(-1,1);
        fichadorPos.z = fichadorPos.z + Random.Range(10, 20.0f) * Random.Range(-1, 1);
        navigator.SetDestination(fichadorPos);
        navigator.transform.position = transform.position;
        _rigidBody.velocity = navigator.desiredVelocity;
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


    public void MoveIt(Vector3 velocity, Vector3 lookAt)
    {

        _rigidBody.velocity = velocity;
    }
}