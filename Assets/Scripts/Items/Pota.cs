using UnityEngine;
using System.Collections;

public class Pota : Item {
    void Start(){
        base.Start();
    }

    protected override void itemAgarrado(){
        Stats levelComponent = Target.gameObject.GetComponent<Stats>();

 	    levelComponent.vida += 15;
        if (levelComponent.vida > levelComponent.health)
            levelComponent.vida = levelComponent.health;

        Debug.Log("Health: " + levelComponent.health);
        Debug.Log("HealthActual: " + levelComponent.vida);

        Destroy(gameObject);
    }
}
