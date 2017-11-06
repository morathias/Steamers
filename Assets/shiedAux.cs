using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shiedAux : MonoBehaviour {

    private Transform capitanPos;
    public GameObject obj;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        capitanPos = obj.transform ;
        transform.position = new Vector3(capitanPos.transform.position.x + 1.0f, capitanPos.transform.position.y+  2.0f, capitanPos.transform.position.z +1.0f);
    }
}
