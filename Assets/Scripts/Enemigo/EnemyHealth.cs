using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;
    public GameObject Potion;
    public GameObject XPerience;
    Vector3 fggt = new Vector3(0, 0.5f, 0);
    public bool dead = false;
    public int totalhealth;
    private Stats prota;

    void Start()
    {
        totalhealth = health;
        prota = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();

    }

    public void applyDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !dead)
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
                int random2 = Random.Range(5, 15);
                prota.VidaActual += random2;
            }
            dead = true;
        }

    }

    public void healDamage(int damage)
    {
        health += damage;

    }
    public bool isDead()
    {
        return dead;

    }
    public int inDanger()
    {
        return (health / totalhealth) * 100;
    }
}

