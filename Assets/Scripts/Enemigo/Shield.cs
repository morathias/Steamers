using UnityEngine;
using System.Collections;

public class Shield : Overlord
{

    Transform fichador;
    public int Rango = 1;
    // float intervalo = 0.2f;
    public int velocidad = 2;
    // float prox = 0.0f;
    // int cartucho = 0;
    //  int cool = 0;
    Transform capitanPos;
    Quaternion neededRotation;
    GameObject Leader;
    //   int ordenPos;
    Infantry troop;
    int damage = 5;
    bool stayput = true;
    float timer = 0;
    float move = 0;
    float timeLeft = 4;
    public Collider shield;
    Vector3 _posicionLider;

    Animator _animations;


    override protected void Start()
    {
        base.Start();
        _stats.applyDamage(1);

        fichador = objective.transform;
        _animations = GetComponent<Animator>();

    }
    void Update()
    {
        switch (_estado)
        {
            case estados.protect:
                {
                    if (troop.GetComponent<Overlord>().dead == true)
                    {
                        Destroy(troop.gameObject);
                        _estado = estados.normal;
                    }
                    if (dead == true)
                    {
                        troop.reset();
                        Destroy(gameObject);
                    }

                    if (Vector3.Distance(transform.position, fichador.position) < 10)
                    {
                        _animations.SetBool("Alert", true);
                        neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                        neededRotation.x = 0;
                        neededRotation.z = 0;

                        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 2.0f);
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

                if (dead == true)
                    Destroy(gameObject);

                if (Vector3.Distance(transform.position, fichador.position) < 10)
                {
                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 2.0f);
                    if (!stayput)
                        _animations.SetBool("Range", true);
                    else
                    {
                        stayput = false;
                        _animations.Play("ready");
                    }
                    transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
                }
                else if (!stayput)
                {

                    _animations.SetBool("Range", false);
                }

                timer += Time.deltaTime * Time.timeScale; ;

                if (timer > 1)
                {

                    searchPro();
                    timer = 0;
                }
                break;

            case estados.fear:
                if (dead == true)
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

                _animations.Play("Walking");
                transform.position = Vector3.MoveTowards(transform.position, _posicionLider, 5 * Time.deltaTime);
                transform.rotation = capitanPos.rotation;
                break;
        }

    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {

    //        Stats healthComponent = collision.gameObject.GetComponent<Stats>();
    //        healthComponent.applyDamage(damage);

    //        stayput = true;
    //    }
    //}


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
            Debug.Log("Bolo");
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
        transform.position = Vector3.MoveTowards(transform.position, _posicionLider, 2 * Time.deltaTime);



    }

    void searchPro()
    {

        _animations.SetBool("Range", false);

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 50);

        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.tag == "EnemyS")
            {
                troop = hit.gameObject.GetComponent<Infantry>();
                if (troop.status() < 30)
                    troop.begin(0, this.gameObject);
                _estado = estados.protect;
                break;
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
    protected override void OnParticleCollision(GameObject other)
    {
        if (other == shield)
        {

            _stats.healDamage(other.GetComponent<DañoBalas>().getDaño());
        }
    }
}
