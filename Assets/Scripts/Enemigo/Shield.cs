using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{

    Transform fichador;
    public int Rango = 50;
    float intervalo = 0.2f;
    float prox = 0.0f;
    int cartucho = 0;
    int cool = 0;
    Transform capitanPos;
    Quaternion neededRotation;
    int ordenPos;

    Vector3 _posicionLider;

    enum estados
    {      //para la maquina de estados
        normal,
        bajoOrdenes
    }

    estados _estado = estados.normal;

    void Start()
    {
        GameObject objective = GameObject.FindGameObjectWithTag("Player");
        fichador = objective.transform;

        GameObject Leader = GameObject.FindGameObjectWithTag("Capitan");
        capitanPos = Leader.transform;

    }
    void Update()
    {
        switch (_estado) { 
            case estados.normal:
                if (Vector3.Distance(transform.position, fichador.position) < Rango)
                {
                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.7f);
                }
                break;

            case estados.bajoOrdenes:

                _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z)-(capitanPos.right*3);

                switch (ordenPos)
                {
                    case 0:
                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.right * 3);
                        break;
                    case 1:
                        _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.right * -3);

                        break;

                }

                transform.position = Vector3.MoveTowards(transform.position, _posicionLider , 5 * Time.deltaTime);
                transform.rotation = capitanPos.rotation;
                break;
        }

    }

    public void timeToHaulYoArses(int pos)
    {
        if (pos < 2){
            ordenPos = pos;
             _estado = estados.bajoOrdenes;
        }
    }
}