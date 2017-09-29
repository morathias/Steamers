using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overlord : MonoBehaviour
{
    protected Prota gratmos;
    protected EnemyHealth _stats;
    protected estados _estado;
    protected int ordenPos;
    public bool dead = true;

    public int daño;


    protected virtual void Start()
    {
        _estado = estados.normal;
        gratmos = Component.FindObjectOfType<Prota>();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (status() <= 0)
        {
            dead = true;
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
}