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
        int random = Random.Range(1, 10);
        health -= damage;
        if (health <= 0)
        {
            if (random > 8)
                Instantiate(Potion, transform.position, transform.rotation);
            Instantiate(XPerience, transform.position - fggt, transform.rotation);

            gameObject.GetComponent<Overlord>().dead = true;

            List<Mision> misiones = GameObject.Find("Tablero").GetComponent<Tablero>().getMisionDeTipo("CazaEnemigos");
            if (misiones.Count > 0)
            {
                for (int i = 0; i < misiones.Count; i++)
                {
                    CazarEnemigos mision = (CazarEnemigos)misiones[i];
                    mision.seMatoEnemigo(gameObject);
                }
            }
        }

    }
}
