using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour {
	public int health = 100;

	public void applyDamage(int damage) {
		health -= damage;
		if (health <= 0) {
           // SceneManager.LoadScene("Menu");
		}
	}

    void Update()
    {


    }
}