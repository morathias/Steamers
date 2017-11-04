using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameDamage : MonoBehaviour {
    int _daño = 100;

    public void setDaño(int daño)
    {
        _daño = daño;
    }
    public int getDaño()
    {
        return _daño;
    }
}
