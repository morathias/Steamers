using UnityEngine;
using System.Collections;

public class Rifleman : MonoBehaviour
{

    public Transform fichador;
    public int Rango = 50;
    float intervalo = 0.2f;
    float prox = 0.0f;
    int cartucho = 0;
    int cool = 0;

    void Start()
    {
        GameObject objective = GameObject.FindGameObjectWithTag("Player");
        fichador = objective.transform;
    }
    void Update()
    {

        if (Vector3.Distance(transform.position, fichador.position) < Rango)
        {
            transform.LookAt(fichador.position);
            

        }

    }
}