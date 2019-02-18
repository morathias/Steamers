using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecisionMaker : MonoBehaviour
{
    public List<Squad> squads = new List<Squad>();

    protected Transform playerTf;

    // Use this for initialization
    void Start()
    {
        if (squads.Count == 0)
        {
            foreach (Squad group in squads)
            {
                group.GetComponent<Squad>().id = squads.IndexOf(group);
            }
        }
        playerTf = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (squads != null)
        {
            foreach (Squad group in squads)
            {
                group.GetComponent<Squad>().setDestination(playerTf.position);

            }
        }
    }
    public void removeSquad(Squad id)
    {
        squads.Remove(id);
        if (squads.Count == 0)
        {
            squads = null;
        }
    }
}
