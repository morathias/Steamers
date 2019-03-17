using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecisionMaker : MonoBehaviour
{
    public List<Squad> squads = new List<Squad>();

    protected GameObject playerGO;
    public Vector3 playerTF;

    // Use this for initialization
    void Start()
    {
        if (squads.Count == 0)
        {
            for (int i = 0; i < squads.Count; i++)
            {
                squads[i].id = squads.IndexOf(squads[i]);
            }
        }
        playerGO = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerTF = playerGO.transform.position;
    }
    public void removeSquad(Squad id)
    {
        squads.Remove(id);
        if (squads.Count == 0)
        {
            squads = null;
        }
    }
    public Vector3 getPos()
    {
        return playerTF;
    }
}
