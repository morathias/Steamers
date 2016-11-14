using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;
    public GameObject Potion;
    public GameObject XPerience;

    void Start()
    {
       
    }

    public void applyDamage(int damage)
    {
        int random = Random.Range(1,10);
        health -= damage;
        if (health <= 0)
        {
            if (random > 8)
                Instantiate(Potion, transform.position, transform.rotation);
                Instantiate(XPerience, transform.position, transform.rotation);
            
            Destroy(gameObject);
        }
    }
}
