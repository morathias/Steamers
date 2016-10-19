using UnityEngine;
using System.Collections;

public class protoScriptAC : MonoBehaviour {

    Transform fichador;
    public int Rango = 50;
    Quaternion neededRotation;


    void Start()
    {
        GameObject objective = GameObject.FindGameObjectWithTag("Player");
        fichador = objective.transform;

    }
    void Update()
    {

        neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.8f);

    }
}
