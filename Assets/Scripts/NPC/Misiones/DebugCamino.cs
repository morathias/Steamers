using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamino : MonoBehaviour {
    
    void OnDrawGizmos()
    {
        GameObject[] puntos;
        puntos = new GameObject[transform.childCount];

        for (int i = 0; i < puntos.Length; i++)
        {
            puntos[i] = transform.GetChild(i).gameObject;
        }
        Gizmos.DrawLine(transform.position, puntos[0].transform.position);
        for (int i = 0; i < puntos.Length; i++){
            if(i+1 < puntos.Length)
                Gizmos.DrawLine(puntos[i].transform.position, puntos[i + 1].transform.position);
        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
