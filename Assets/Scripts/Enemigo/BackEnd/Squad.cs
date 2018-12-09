using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Squad : MonoBehaviour {
    public List<GameObject> soldiers = new List<GameObject>();
	// Use this for initialization
	void Start () {
        if (soldiers.Count == 0)
        {
            foreach (GameObject unit in soldiers)
            {
                unit.GetComponent<Overlord>().id = soldiers.IndexOf(unit);
            }
        }

	}
	
	// Update is called once per frame
    public void registerUnit(GameObject newUnit)
    {
        soldiers.Add(newUnit);
        newUnit.GetComponent<Overlord>().id = soldiers.IndexOf(newUnit);
    }

    public void removeUnit(int id)
    {
        soldiers.Remove(soldiers[id]);
        if (soldiers.Count == 0)
        {
            soldiers = null;
        }
    }
    public void emptySquad()
    {
        soldiers.Clear();
        soldiers = null;
    }

    public void setDestination(Vector3 Objective)
    {
        if (soldiers[0].GetComponent<Overlord>().setDestination(Objective))
        {
            foreach (GameObject unit in soldiers.Skip(1))
            {
                unit.GetComponent<Overlord>().setDestination(Objective);
            }
        }

    }
}
