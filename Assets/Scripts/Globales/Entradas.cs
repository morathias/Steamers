using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entradas : MonoBehaviour {

    public EsceneManager esceneManager;
    public int index;

    void OnTriggerEnter(Collider player) {
        if (player.tag == "Player") {
            esceneManager.cambiarEscena(index);
        }
    }
}
