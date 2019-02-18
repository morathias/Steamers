using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Squad : MonoBehaviour
{
    public List<GameObject> soldiers = new List<GameObject>();
    public int id;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public void registerUnit(GameObject newUnit)
    {
        soldiers.Add(newUnit);
        newUnit.GetComponent<Overlord>().id = soldiers.IndexOf(newUnit);
    }

    public void removeUnit(GameObject id)
    {
        soldiers.Remove(id);
        if (soldiers.Count == 0)
        {
            emptySquad();
        }
    }
    public void emptySquad()
    {
        soldiers.Clear();
        soldiers = null;
        GetComponentInParent<DecisionMaker>().removeSquad(gameObject.GetComponent<Squad>());
        this.enabled = false;
    }

    public bool setDestination(Vector3 Objective)
    {
        for (int i = 0; i < soldiers.Count; i++)
        {
            if (soldiers[i].GetComponent<Overlord>().setDestination(Objective) == false)
            {
                return false;
            }
        }
        return false;
    }
}
