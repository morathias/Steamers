using UnityEngine;
using System.Collections;

public class ExpCube : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Stats expComponent = collision.gameObject.GetComponent<Stats>();
            if(Variables.choose)
                expComponent.exp = 100;

            Destroy(gameObject);
        }
    }
}
