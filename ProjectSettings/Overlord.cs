using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overlord : MonoBehaviour
{
    protected Prota gratmos;
    protected EnemyHealth _stats;
    protected estados _estado;
    protected int ordenPos;
    public bool dead = false;

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
        bajoOrdenes
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void reset()
    {
        _estado = estados.normal;
    }

    protected virtual void OnParticleCollision(GameObject other)
    {
        if (other.transform.tag == "BalaPlayer")
        {
            _stats.applyDamage(other.GetComponent<DañoBalas>().getDaño());
        }
    }
}