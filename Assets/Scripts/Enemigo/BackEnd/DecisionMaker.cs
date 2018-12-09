using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionMaker : MonoBehaviour {
    public List<Squad> squads = new List<Squad>();
    protected Transform playerTf;

    // Use this for initialization
    void Start() {
        playerTf = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        foreach (Squad group in squads)
        {
            group.GetComponent<Squad>().setDestination(playerTf.position);
        }
    }
}
