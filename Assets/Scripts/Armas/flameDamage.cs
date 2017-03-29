using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameDamage : MonoBehaviour {
    int _daño = 1;

    public void setDaño(int daño)
    {
        _daño = daño;
    }
    public int getDaño()
    {
        return _daño;
    }
}
