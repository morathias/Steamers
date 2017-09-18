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
    bool stayput = false;
    int timer = 0;
    int move = 0;


    Vector3 _posicionLider;


    override protected void Start()
    {
        base.Start();
        _stats.applyDamage(1);
        GameObject objective = GameObject.FindGameObjectWithTag("Player");
        fichador = objective.transform;

    }
    void Update()
    {

        switch (_estado)
        {
            case estados.protect:
                {
                    if (dead == true)
                    {
                        troop.reset();
                        Destroy(gameObject);
                    }
                    if (Vector3.Distance(transform.position, fichador.position) < 10)
                    {
                        neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                        neededRotation.x = 0;
                        neededRotation.z = 0;

                        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.75f);
                        moveIt();
                        move++;
                    }
                }
                    break;
            case estados.normal:
                if (dead == true)
                {
                    Destroy(gameObject);
                }

                if (Vector3.Distance(transform.position, fichador.position) < Rango)
                {
                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.75f);
                }
                transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

                    timer++;

                if (timer > 60)
                {

                    searchPro();
                    timer = 0;
                }



                break;

            case estados.bajoOrdenes:

                _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.right * 3);

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

                transform.position = Vector3.MoveTowards(transform.position, _posicionLider, 5 * Time.deltaTime);
                transform.rotation = capitanPos.rotation;
                break;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Stats healthComponent = collision.gameObject.GetComponent<Stats>();
            healthComponent.applyDamage(damage);

            stayput = true;
        }
    }

    public void reset()
    {
        _estado = estados.normal;
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
            Debug.Log("Bolo");
            return false;
        }
    }
    void moveIt()
    {
        if (troop.GetComponent<Overlord>().deader() == true)
            _estado = estados.normal;
        else
        {
            move++;
            if (move > 90)
            {
                _posicionLider = transform.position;
                _posicionLider.x += Random.Range(-20.0f, 20.0f);
                _posicionLider.z += Random.Range(-20.0f, 20.0f);
                move = 0;

            }
            transform.position = Vector3.MoveTowards(transform.position, _posicionLider, 2 * Time.deltaTime);

        }

    }

    void searchPro()
    {

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 50);
        _estado = estados.bajoOrdenes;

        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.tag == "EnemyS")
            {
                troop = hit.gameObject.GetComponent<Infantry>();
                if (troop.status()<30)
                troop.begin(0, this.gameObject);
            }

        }
        _estado = estados.protect;
    }
}
