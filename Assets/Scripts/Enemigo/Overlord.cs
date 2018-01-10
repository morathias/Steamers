using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overlord : MonoBehaviour
{
    protected UnityEngine.AI.NavMeshAgent navigator;
    protected GameObject objective;
    protected Transform fichador;
    protected Vector3 fichadorPos;
    protected EnemyHealth _stats;
    public estados _estado;
    protected int ordenPos;
    public int countdown = 6;
    public int daño;
    public string phase = "0";
    public ParticleSystem blood;
    protected Quaternion neededRotation;
    protected Rigidbody _rigidBody;
    protected ParticleSystem _balaE;

    protected virtual void Start()
    {
        _estado = estados.normal;
        objective = GameObject.FindGameObjectWithTag("Player");
        _stats = GetComponent<EnemyHealth>();
        fichador = objective.transform;
        fichadorPos = fichador.position;
        
    }
    public enum estados
    {
        normal,
        bajoOrdenes,
        recarga,
        apuntando,
        rage,
        fear,
        protect,
        Durazno,
        brag,
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown < 1)
        {
            phase = "1";
        }
    }

    public void reset()
    {
        _estado = estados.normal;
    }

    public void fear()
    {
        _estado = estados.fear;
    }

    protected virtual void moveIt(float chaos)
    {
        neededRotation = Quaternion.LookRotation(fichador.position - transform.position);
        neededRotation *= Quaternion.Euler(0, Random.Range(90.0f, 270.0f), 0);
        neededRotation.x = 0;
        neededRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 5f);

        fichadorPos.x = fichadorPos.x + Random.Range(10, 20.0f) * Random.Range(-1, 1);
        fichadorPos.z = fichadorPos.z + Random.Range(10, 20.0f) * Random.Range(-1, 1);
        navigator.SetDestination(fichadorPos);
        navigator.transform.position = transform.position;
        _rigidBody.velocity = navigator.desiredVelocity;
    }
    protected virtual void OnParticleCollision(GameObject other)
    {
        if (other.transform.tag == "BalaPlayer")
        {
            // _stats.applyDamage(10000);

            _stats.applyDamage(other.GetComponent<DañoBalas>().getDaño());

        }

        if (other.transform.tag == "FuegoPlayer")
        {
            _stats.applyDamage(other.GetComponent<flameDamage>().getDaño());
        }
        blood.Emit(1);
    }
    public int status()
    {
        return _stats.inDanger();
    }
    public string bossSat()
    {
        return phase;
    }
}