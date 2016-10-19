using UnityEngine;
using System.Collections;

public class Infantry : MonoBehaviour
{

    Transform fichador;
    public int Rango = 50;
    float intervalo = 0.2f;
    float prox = 0.0f;
    int limite = 60;
    int cool = 0;
    Transform capitanPos;
    public ParticleSystem _balaE;
    Quaternion neededRotation;
    
    Vector3 _posicionLider;

    enum estados
    {     
        normal,
        bajoOrdenes
    }

    estados _estado = estados.bajoOrdenes;

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
                    transform.LookAt(fichador.position);
                }
                break;

            case estados.bajoOrdenes:

                _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z)-(capitanPos.forward*3);

                transform.position = Vector3.MoveTowards(transform.position, _posicionLider , 5 * Time.deltaTime);
                limite++;
                if (Vector3.Distance(transform.position, fichador.position) < Rango)
                {
                    neededRotation = Quaternion.LookRotation(fichador.transform.position - transform.position);

                    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 0.9f);
                }

                if (limite > 60)
                    fire();

                    
                break;
        }


    }

    void fire()
    {
        _balaE.startLifetime = Rango / _balaE.startSpeed;
        RaycastHit Bang;
        _balaE.transform.position = transform.position;
        _balaE.Emit(1);
        if (Physics.Raycast(transform.position, transform.forward, out Bang, 15))
        {
            if (Bang.transform.tag == "Player")
            {

                Debug.Log("Can you feel eet?!");

                Stats healthComponent = Bang.collider.gameObject.GetComponent<Stats>();

                healthComponent.applyDamage(5);
            }
        }
        limite = 0;
    }

}