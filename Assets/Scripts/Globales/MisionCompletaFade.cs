using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MisionCompletaFade : MonoBehaviour {
    float timer = 7f;

	void Update () {
        if (gameObject.GetComponent<Text>().enabled)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = 7f;
                gameObject.GetComponent<Text>().enabled = false;
                SceneManager.LoadScene(0);
            }
        }
	}
}
