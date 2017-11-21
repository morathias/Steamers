using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overlord : MonoBehaviour
{
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