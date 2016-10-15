using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int health = 100;

    void Start()
    {

    }

    public void applyDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
