using UnityEngine;
using System.Collections;

public class Pota : MonoBehaviour {

    public Transform Target;
    public int RangoDeteccion;
    public int velocidad;
    private Stats levelComponent;

    void Start(){
        GameObject Objetivo = GameObject.FindGameObjectWithTag("Player");
        Target = Objetivo.transform;
        levelComponent = GameObject.Find("Prota").gameObject.GetComponent<Stats>();
    }
    void Update(){
        if (levelComponent.vida >= levelComponent.health){
            
        }
        if (Vector3.Distance(transform.position, Target.position) < RangoDeteccion && levelComponent.vida < levelComponent.health)
        {
            transform.LookAt(Target.position);
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider collision)
    {

         if (collision.gameObject.tag == "Player" && levelComponent.vida < levelComponent.health)
         {

            levelComponent.vida += 15;
            Debug.Log("Health: " + levelComponent.health);
            Debug.Log("HealthActual: " + levelComponent.vida);

            Destroy(gameObject);
        }
    }
}
