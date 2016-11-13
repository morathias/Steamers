using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stats : MonoBehaviour {
	public int health = 100;
    private int _vidaActual = 100;
    public int exp = 0;
    private int level = 1;
    public int damage = 1;
    public int atkspeed = 10;
    public int stat = 0;
    public Text stats;
    public Text levelsT;
    public LevelUp _levelUp;

	public bool applyDamage(int damage) {
		_vidaActual -= damage;
        if (_vidaActual <= 0)
            return true;
        return false;
	}

    public int vida{
        get{
            return _vidaActual;
        }
        set{
            _vidaActual = value;
        }
    }

    void Update()
    {
        if (exp == 100){
            level++;
            _levelUp.activar(true);
            exp = 0;
            Variables.random1 = Random.Range(1, 4);
            Variables.random2 = Random.Range(1, 4);
            stat ++;
        }
        if (stat > 0)
            stats.text = "" + stat;
        else
            stats.text = "";

        levelsT.text = "Lvl: " + level;
    }
}