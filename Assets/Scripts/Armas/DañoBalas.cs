using UnityEngine;
using System.Collections;

public class DañoBalas : MonoBehaviour {
    int _daño;

    public void setDaño(int daño) {
        _daño = daño;
    }
    public int getDaño() {
        return _daño;
    }
}
