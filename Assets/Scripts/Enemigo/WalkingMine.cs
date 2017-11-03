using UnityEngine;
using System.Collections;

public class WalkingMine : Overlord {

    public Transform Target;
    public int RangoDeteccion;
    public int velocidad;
    public int damage;
    int timer = 0;
    bool move = false;


    override protected void Start()
    {
        base.Start();
        GameObject Objetivo = GameObject.FindGameObjectWithTag("Player");
        Target = Objetivo.transform;

    }

    void Update()
    {
        if (dead == true)
            Destroy(gameObject);
        if (Vector3.Distance(transform.position, Target.position) < RangoDeteccion)
            move = true;
        if (move == true){
            transform.LookAt(Target.position);
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Stats healthComponent = collision.gameObject.GetComponent<Stats>();
            Prota stun = collision.gameObject.GetComponent<Prota>();

            stun.stunE();
            healthComponent.VidaActual -= 15;
  
            Destroy(gameObject);
        }
    }
  }

