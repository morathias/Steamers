using UnityEngine;
using System.Collections;

public class protoScriptAC : MonoBehaviour
{

    Transform fichador;
    public int Rango = 50;
    Quaternion neededRotation;
    private int commandTime = 0;
    private bool onOrder = false;

    void Start()
    {
        GameObject objective = GameObject.FindGameObjectWithTag("Player");
        fichador = objective.transform;

    }
    void Update()
    {
        if (commandTime > 90)
        {
            commandTime = 0;
            formUp();
        }

        neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
        neededRotation.x = 0;
        neededRotation.z = 0;   
     
        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.8f);

        if (onOrder == false)
        {
            commandTime++;
        }
    }

    void formUp()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 50);
        int numeroSoldado = 0;
        int numeroEscudo = 0;
        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.tag == "Enemy")
            {
                Infantry rifleman = hit.GetComponent<Infantry>();
                rifleman.timeToHaulYoArses(numeroSoldado);
                numeroSoldado++;
            }
            if (hit.gameObject.tag == "ArmoredE")
            {
                Shield armored = hit.GetComponent<Shield>();
                armored.timeToHaulYoArses(numeroEscudo);
                numeroEscudo++;
            }
            onOrder = true;
        }
    }
}