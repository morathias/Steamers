using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class protoScriptAC : Overlord
{

    Transform fichador;
    public int Rango = 1;
    Quaternion neededRotation;
    private int commandTime = 0;
    private int commandInit;
    private bool onOrder = false;
    Vector3 _posicionLider;
    float move = 0;
    int limite = 0;
    public ParticleSystem _balaE;
    private List<GameObject> unidades;
    int count;
    Infantry troop;
    Shield sTroop;
    int numeroSoldado = 0;
    int numeroEscudo = 0;

    override protected void Start()
    {
        base.Start();
        unidades = new List<GameObject>();
        _stats.applyDamage(1);
        fichador = objective.transform;
        commandInit = Random.Range(120, 500);
    }

    void Update()
    {
        switch (_estado)
        {
            case estados.normal:
                if (dead == true)
                    Destroy(gameObject);

                if (commandTime > commandInit)
                {
                    commandTime = 0;
                    formUp();
                }
                limite++;
                if (limite > 80)
                    fire();
                commandTime++;
                moveIt();

                break;

            case estados.bajoOrdenes:
                if (dead == true)
                {
                    count = unidades.Count;
                    for (int f = 0; f < count; f++)
                    {
                        unidades[f].GetComponent<Overlord>().fear();
                        count = unidades.Count;
                    }

                    Destroy(gameObject);
                }
                limite++;
                if (limite > 70)
                {
                    fire();
                }

                check();
                moveIt();

                break;
        }
    }
    void formUp()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 50);
        
        _estado = estados.bajoOrdenes;

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
        check();
    }

    void moveIt()
    {
        neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
        neededRotation.x = 0;
        neededRotation.z = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.95f);
        _posicionLider = transform.position;
        if (Vector3.Distance(transform.position, fichador.position) < Rango)
        {
            neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.75f);
        }
        transform.Translate(Vector3.forward * 1 * Time.deltaTime);
    }

    void fire()
    {
        _balaE.startLifetime = Rango / _balaE.startSpeed;

        _balaE.transform.position = transform.position;
        _balaE.Emit(30);

        limite = 0;
    }

    void check()
    {
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
            numeroSoldado = 0;
            numeroEscudo = 0;
            count = 0;
            _estado = estados.normal;
        }

        else for (int i = 0; i < unidades.Count; i++)
            {

                if (unidades[i].GetComponent<Overlord>().dead == true)
                {
                    Destroy(unidades[i]);
                    unidades.RemoveAt(i);
                    count = unidades.Count;
                }
            }
    }
}