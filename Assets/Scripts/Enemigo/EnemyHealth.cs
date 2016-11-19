using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
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
            List<Mision> misiones = GameObject.Find("Tablero").GetComponent<Tablero>().getMisionDeTipo("CazaEnemigos");
            if (misiones.Count > 0)
            {
                for (int i = 0; i < misiones.Count; i++)
                {
                    CazarEnemigos mision = (CazarEnemigos)misiones[i];
                    mision.seMatoEnemigo(gameObject);
                }
            }

            Destroy(gameObject);
        }
    }
}
