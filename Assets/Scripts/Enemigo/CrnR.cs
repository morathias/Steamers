using UnityEngine;
using System.Collections;

public class CrnR : Overlord
{

    Transform Target;
    int RangoBusqueda = 30;
    int RangoRush = 10;
    bool stayput = false;
    int timer = 0;
    int cool = 0;
    int velocidad = 0;


    override protected void Start()
    {
        base.Start();
        GameObject Objetivo = GameObject.FindGameObjectWithTag("Player");
        Target = Objetivo.transform;
    }

    void Update()
    {
        if (dead)
        {
            Destroy(gameObject);
        }
        switch (_estado)
        {
            case estados.normal:
                timer++;

                if (Vector3.Distance(transform.position, Target.position) < RangoBusqueda)
                {

                    transform.LookAt(Target.position);
                    transform.Translate(Vector3.forward * 3 * Time.deltaTime);
                }

                if (Vector3.Distance(transform.position, Target.position) < RangoRush && timer > 60)
                {
                    timer = 0;
                    _estado = estados.apuntando;
                }

                break;

            case estados.rage:
                transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
                timer++;
                velocidad += 4;
                if (timer > 20)
                {
                    velocidad = 0;
                    timer = 0;
                    cool = 45;
                    _estado = estados.Durazno;
                }

                break;

            case estados.Durazno:
                timer++;
                if (timer > cool)
                {
                    timer = 0;
                    _estado = estados.normal;
                }


                break;

            case estados.apuntando:
                transform.LookAt(Target.position);

                RaycastHit ICU;

                if (Physics.Raycast(transform.position, transform.forward, out ICU) && ICU.transform.tag == "Player")
                    _estado = estados.rage;
                Debug.Log("Bollocks");
                break;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Stats healthComponent = collision.gameObject.GetComponent<Stats>();
            healthComponent.applyDamage(daño);

            stayput = true;
        }
    }
}