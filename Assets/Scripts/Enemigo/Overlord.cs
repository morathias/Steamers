using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overlord : MonoBehaviour
{
    protected GameObject objective;
    protected EnemyHealth _stats;
    protected estados _estado;
    protected int ordenPos;
    public bool dead = false;
    public int countdown = 6;
    public int daño;
    public string phase = "0";

    protected virtual void Start()
    {
        _estado = estados.normal;
        objective = GameObject.FindGameObjectWithTag("Player");
        _stats = GetComponent<EnemyHealth>();
    }
    protected enum estados
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
        if (countdown<1)
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
            _stats.applyDamage(other.GetComponent<DañoBalas>().getDaño());
        }

        if (other.transform.tag == "FuegoPlayer")
        {
            _stats.applyDamage(other.GetComponent<flameDamage>().getDaño());
        }
    }
    public int status()
    {
        return _stats.health;
    }
    public string bossSat()
    {
        return phase;
    }
}