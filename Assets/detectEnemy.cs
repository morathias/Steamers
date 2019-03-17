using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectEnemy : MonoBehaviour {
    public GameObject shielder;
    private Shield shieldComp;
    	// Use this for initialization
	void Start () {
        shieldComp = shielder.GetComponent<Shield>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Bash()
    {

          
            shieldComp.Attack();
        
    }
}
