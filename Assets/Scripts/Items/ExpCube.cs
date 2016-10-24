using UnityEngine;
using System.Collections;

public class ExpCube : MonoBehaviour {

    public Transform Target;
    public int RangoDeteccion;
    public int velocidad;

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
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Stats expComponent = collision.gameObject.GetComponent<Stats>();
            expComponent.exp = 100;

            Destroy(gameObject);
        }
    }
}
