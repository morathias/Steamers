﻿using UnityEngine;
using System.Collections;

public class WalkingMine : MonoBehaviour {

    public Transform Target;
    public int RangoDeteccion;
    public int velocidad;
    public int damage;
    int timer = 0;
    void Start()
    {
        GameObject Objetivo = GameObject.FindGameObjectWithTag("Player");
        Target = Objetivo.transform;

    }

    void Update()
    {
        if (Vector3.Distance(transform.position, Target.position) < RangoDeteccion)
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Stats healthComponent = collision.gameObject.GetComponent<Stats>();
            Stats expComponent = collision.gameObject.GetComponent<Stats>();
            Prota stun = collision.gameObject.GetComponent<Prota>();

            stun.stunE();
            healthComponent.applyDamage(damage);
            if (Variables.choose)
                expComponent.exp += 10;
  
            Destroy(gameObject);

        }
    }
  }

