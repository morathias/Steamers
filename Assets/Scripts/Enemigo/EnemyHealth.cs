using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;
    public GameObject Potion;
    public GameObject XPerience;
    Vector3 fggt = new Vector3(0, 0.5f, 0);
    Overlord killswitch;
    public int totalhealth;
    private Stats prota;

    void Start()
    {
        totalhealth = health;
        killswitch = this.GetComponent<Overlord>();
        prota = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();

    }

    public void applyDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !killswitch.dead)
        {
            int random = Random.Range(1, 10);
            if (random >= 7)
                Instantiate(Potion, transform.position + fggt, transform.rotation);
            Instantiate(XPerience, transform.position + fggt + new Vector3(0.5f, 0, 0), transform.rotation);
            if (prota.RageOn)
            {
                prota.rage++;
                prota.timerRage = 0;
            }
            if (prota.onKilling)
            {
                int random2 = Random.Range(1, 10);
                prota.VidaActual += random2;
            }
            killswitch.dead = true;
        }

    }

    public void healDamage(int damage)
    {
        health += damage;

    }
}

