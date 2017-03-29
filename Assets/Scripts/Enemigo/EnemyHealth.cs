using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;
    public GameObject Potion;
    public GameObject XPerience;
    Vector3 fggt = new Vector3 (1,1,1);

    void Start()
    {

    }

    public void applyDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            int random = Random.Range(1, 10);
            if (random > 8)
                Instantiate(Potion, transform.position, transform.rotation);
            Instantiate(XPerience, transform.position - fggt, transform.rotation);

            Destroy(gameObject);
        }
    }
}
