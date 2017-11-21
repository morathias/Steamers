using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : Overlord

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
    public GameObject brag1;
    public GameObject brag2;
    Animator _animations;
    Vector3 specific;
    

    override protected void Start()
    {
        base.Start();
        _stats.applyDamage(1);

        fichador = objective.transform;
        _animations = GetComponent<Animator>();
        _estado = estados.protect;

        specific = new Vector3(brag1.transform.position.x, 0, brag1.transform.position.z);

    }
    void Update()
    {
        switch (_estado)
        {
            case estados.protect:
                {


                        _animations.SetBool("Alert", true);

                    if (bossSat() == "1")
                    {
                        _animations.Play("Walking");
                        transform.position = Vector3.MoveTowards(transform.position, specific, 3 * Time.deltaTime);
                       
                    }
                     if (transform.position == specific)
                    {
                     ///   transform.LookAt(brag2.transform);
                        _estado = estados.Durazno;
                    }
                }
                break;
            case estados.Durazno:
                transform.LookAt(brag2.transform);
                break;
            case estados.normal:


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


            _stats.healDamage(other.GetComponent<DañoBalas>().getDaño());
        
    }
}

