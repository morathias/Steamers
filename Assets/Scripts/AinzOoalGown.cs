using UnityEngine;
using System.Collections;

public class Overlord : MonoBehaviour {
    Prota bla = Component.FindObjectOfType<Prota>();
    int totalFggts = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (totalFggts < 0)
        {
            bla.fear();
        }
	}

    public void OhCrapOhCrap()
    {
        totalFggts--;
    }
    public void hoihoihoihoi()
    {
        totalFggts++;
    }
}
