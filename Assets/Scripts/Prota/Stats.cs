using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour {
	public int health = 100;
    private int _vidaActual = 100;
    public int exp = 0;
    public int level = 0;
    public int damage = 1;
    public int atkspeed = 10;

	public void applyDamage(int damage) {
		health -= damage;
		if (health <= 0) {
           // SceneManager.LoadScene("Menu");
		}
	}

    void Update()
    {
        if (exp == 100 && Variables.choose == true){
            level++;
            exp = 0;
            Variables.choose = false;
            Variables.random1 = Random.Range(1, 4);
            Variables.random2 = Random.Range(1, 4);
            Variables.limitador = 1;
            Debug.Log(level);
        }
    }
}