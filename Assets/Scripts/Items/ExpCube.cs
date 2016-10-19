using UnityEngine;
using System.Collections;

public class ExpCube : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Wololo1");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Wololo2");
            Stats expComponent = collision.gameObject.GetComponent<Stats>();
            expComponent.exp = 100;

            Destroy(gameObject);
        }
    }
}
