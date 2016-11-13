using UnityEngine;
using System.Collections;

public class CualquierTecla : MonoBehaviour {
    public GameObject playBtn;
    public GameObject cerrarBtn;

	void Update () {
        if (Input.anyKeyDown) {
            playBtn.SetActive(true);
            cerrarBtn.SetActive(true);
            gameObject.SetActive(false);
        }
	}
}
