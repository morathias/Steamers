using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESMachine : MonoBehaviour
{
    public int[,] stateGrid;

    int currentState = 0;

    public void init(int stateAmount, int eventAmount)
    {
        stateGrid = new int[stateAmount, eventAmount];

        for (int x = 0; x < stateAmount; x++)
        { //AsignarValorDeinicio
            for (int y = 0; y < eventAmount; y++)
            {
                stateGrid[x, y] = -1;
            }
        }
    }

    public int getState()
    {
        return currentState;
    }

    public State setEvent(int checkEvent)
    {
        int eventTS = stateGrid[currentState, checkEvent]; //CargarValorDeEvent
        Debug.Log("event" + eventTS);
        if (eventTS != -1) //Si el valor de evento no es vacio, cargar como actual.
        {
            Debug.Log("Aca estoy en State");
            currentState = eventTS;

        }
        return (State)currentState;
    }

    public void relation(int currState, int newEvent, int newState)
    {
        stateGrid[currState, newEvent] = newState;
    }

}
