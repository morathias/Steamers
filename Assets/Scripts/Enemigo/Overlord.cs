using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overlord : Formations {

    private int totalF;
    protected bool holdIt = false;
    protected Prota gratmos;
    protected EnemyHealth _stats;
    protected List<GameObject> unidades;
    protected List<List<GameObject>> grupos; //Oh Kelaah... The fuck I'm freestylin' here? Why it works?! CARMACK HELP ME!
    protected Formations orders;

    protected virtual void Start()
    {
        gratmos = Component.FindObjectOfType<Prota>();
        _stats = GetComponent<EnemyHealth>();
        unidades = new List<GameObject>();
        grupos = new List<List<GameObject>>();
    }

    void Update()
    {

        Debug.Log(totalF);
        Debug.Log("Cantidad");
        if (holdIt == true)
            check();
        
    }


    public void hoihoihoihoi(GameObject troop, int pos)
    {
        unidades.Add(troop);
        troop.gameObject.GetComponent<Formations>().begin(pos);
        totalF=+1;
   
        if (totalF > 1)
        {
            
            grupos.Add(unidades);
            unidades.Clear();
            holdIt = true;
        }
    }

    public int getF()
    {
        return totalF;
    }

    protected virtual void OnParticleCollision(GameObject other)
    {
        if (other.transform.tag == "BalaPlayer")
        {
            _stats.applyDamage(other.GetComponent<DañoBalas>().getDaño());
        }
    }

    protected virtual void check()
    {
       
        for (int i = 0; i < grupos.Count; i++)
        {
            if (grupos[i].Count < 4)
            {
                for (int f = 0; f < grupos[i].Count; f++)
                {
                    grupos[i][f].GetComponentInParent<Formations>().reset();
                    
                }

            }
        }
    }
}
