using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overlord : MonoBehaviour
{
    protected GameObject objective;
    protected Transform fichador;
    protected Vector3 playerTF;
    protected EnemyHealth _stats;
    public estados _estado;
    protected int ordenPos;
    public int countdown = 6;
    public int daño;
    public string phase = "0";
    public ParticleSystem blood;
    protected UnityEngine.AI.NavMeshAgent navigator { get; private set; }
    public int id;

    protected virtual void Start()
    {
        _estado = estados.normal;
        objective = GameObject.FindGameObjectWithTag("Player");
        _stats = GetComponent<EnemyHealth>();
        fichador = objective.transform;
        playerTF = fichador.position;

        navigator = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
       // navigator.isStopped = true;
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
    public bool setDestination(Vector3 objective)
    {
        if (!navigator.isStopped) //Se asegura que este activado, si ya esta, por diseño, implicaria que el jugador esta a distancia.
        {

            navigator.isStopped = false;
            navigator.destination = objective;

            if (navigator.remainingDistance > 30)
            {

                navigator.isStopped = true;
                return false;
            }
            else
            {
                Debug.Log("holllaaa");
                playerTF = objective;
                return true;
            }
        }

        return true;
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