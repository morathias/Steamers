using UnityEngine;
using System.Collections;

public class protoScriptAC : Overlord
{

    Transform fichador;
    public int Rango = 50;
    Quaternion neededRotation;
    private int commandTime = 0;
    private bool onOrder = false;
    Vector3 _posicionLider;
    int move = 0;
    int limite = 0;
    public ParticleSystem _balaE;


    override protected void Start()
    {
        base.Start();
        _stats.applyDamage(1);
        GameObject objective = GameObject.FindGameObjectWithTag("Player");
        fichador = objective.transform;

    }
    void Update()
    {

        if (commandTime > 90)
        {
            commandTime = 0;
            onOrder = true;
            formUp();
        }

        neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
        neededRotation.x = 0;
        neededRotation.z = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.95f);

        limite++;
        if (limite > 60)
            fire();

        if (onOrder == false)
        {
            commandTime++;
        }


        moveIt();
    }

    void formUp()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 50);
        int numeroSoldado = 0;
        int numeroEscudo = 0;
        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.tag == "Enemy")
            {
                Infantry rifleman = hit.GetComponent<Infantry>();
                rifleman.timeToHaulYoArses(numeroSoldado);
                numeroSoldado++;
            }
            if (hit.gameObject.tag == "ArmoredE")
            {
                Shield armored = hit.GetComponent<Shield>();
                armored.timeToHaulYoArses(numeroEscudo);
                numeroEscudo++;
            }
            onOrder = true;
        }
        if (move > 90)
            moveIt();
    }

    void moveIt()
    {
        _posicionLider = transform.position;
        if (move > 90)
        {
            move = 0;

            _posicionLider.x += Random.Range(-20.0f, 20.0f);
            _posicionLider.z += Random.Range(-20.0f, 20.0f);
        }
        transform.position = Vector3.MoveTowards(transform.position, _posicionLider, 5 * Time.deltaTime);

    }

    void fire()
    {
        _balaE.startLifetime = Rango / _balaE.startSpeed;

        _balaE.transform.position = transform.position;
        _balaE.Emit(30);

        limite = 0;
    }
}