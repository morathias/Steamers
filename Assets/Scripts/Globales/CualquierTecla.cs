using UnityEngine;
using System.Collections;

public class CualquierTecla : MonoBehaviour {
    public GameObject[] buttons;

	void Update () {
        if (Input.anyKeyDown) {
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].SetActive(true);

            gameObject.SetActive(false);
        }
	}
}
