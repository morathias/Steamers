using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;
    public GameObject Potion;
    public GameObject XPerience;
    Vector3 fggt = new Vector3 (1,1,1);
    Overlord killswitch;
    public int totalhealth;


    void Start()
    {
        totalhealth = health;
        killswitch = GetComponent<Overlord>();
    }

    public void applyDamage(int damage)
    {
        health -= damage;
        Debug.Log(killswitch.dead);
        if (health <= 0 && !killswitch.dead)
        {
            int random = Random.Range(1, 10);
            if (random > 8)
                Instantiate(Potion, transform.position, transform.rotation);
            Instantiate(XPerience, transform.position - fggt, transform.rotation);
            killswitch.dead = true;
        }

    }
}
