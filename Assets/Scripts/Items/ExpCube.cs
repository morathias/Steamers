using UnityEngine;
using System.Collections;

public class ExpCube : Item {

    void Start(){
        base.Start();
    }

    protected override void itemAgarrado()
    {
        Stats expComponent = Target.gameObject.GetComponent<Stats>();
        expComponent.exp += 100;
        Debug.Log(expComponent.exp);
        Destroy(gameObject);
    }
}
