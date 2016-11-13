using UnityEngine;
using System.Collections;

public class Overlord : MonoBehaviour {
    int totalFggts = 0;
    protected Prota bla;
    protected EnemyHealth _stats;
    
    public int daño;
    
    protected virtual void Start()
    {
        bla = Component.FindObjectOfType<Prota>();
        _stats = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (totalFggts < 0)
        {
            bla.fear();
        }
    }

    public void OhCrapOhCrap()
    {
        totalFggts--;
    }
    public void hoihoihoihoi()
    {
        totalFggts++;
    }

    public int getFggts()
    {
        return totalFggts;
    }

    protected virtual void OnParticleCollision(GameObject other)
    {
        if (other.transform.tag == "BalaPlayer")
        {
            _stats.applyDamage(other.GetComponent<DañoBalas>().getDaño());
        }
    }
}
