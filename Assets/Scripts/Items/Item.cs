using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    protected Transform Target;
    public int RangoDeteccion;
    public int velocidad;

    public enum Tipo {
        consumible,
        cobre,
        hierro,
        oro,
        quest
    }
    public Tipo tipo;

	protected void Start () {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void Update () {
        if (Vector3.Distance(transform.position, Target.position) < RangoDeteccion){
            transform.LookAt(Target.position);
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        }
	}

    protected virtual void itemAgarrado() { }

    void OnTriggerEnter(Collider collision){
        if (collision.gameObject.tag == "Player")
        {
            EncontrarItem.onItemAgarrado(this);
            itemAgarrado();
        }
    }
}
