using UnityEngine;
using System.Collections;

public class CQCMob : Overlord
{

    Transform Target;
    public int RangoLucha;
    public int velocidad = 10;
    public int damage;
    bool stayput = false;
    int timer = 0;

    override protected void Start()
    {
        base.Start();
        GameObject Objetivo = GameObject.FindGameObjectWithTag("Player");
        Target = Objetivo.transform;
    }

    void Update()
    {
        if (stayput == false)
        {

            if (Vector3.Distance(transform.position, Target.position) < RangoLucha)
            {
                transform.LookAt(Target.position);
                transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
            }
            //if (Vector3.Distance(transform.position, fichador.position) < RangoBusca && Vector3.Distance(transform.position, fichador.position) > RangoLucha)
            //{
            //    transform.LookAt(fichador.position);
            //    transform.Translate(Vector3.forward * 17 * Time.deltaTime);
            //}
            //if (Vector3.Distance(transform.position, fichador.position) > RangoBusca)
            //    transform.Translate(Vector3.forward * 20 * Time.deltaTime);
        }

        if (stayput == true)
        {
            timer++;
        }

        if (timer > 60)
        {

            stayput = false;
            timer = 0;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Stats healthComponent = collision.gameObject.GetComponent<Stats>();
            healthComponent.applyDamage(damage);

            stayput = true;
        }
    }
}