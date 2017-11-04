using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stats : MonoBehaviour {
    public int health = Variables.Ghealth;
    private int _vidaActual;
    public int exp;
    private int level;
    public int damage;
    public int atkspeed;
    public int stat;
    public Text stats;
    public Text levelsT;
    public Text levelUpTxt;
    public LevelUp _levelUp;

    public bool applyDamage(int damage)
    {
        _vidaActual -= damage;
        if (_vidaActual <= 0)
            return true;
        return false;
    }

    public int vida
    {
        get
        {
            return _vidaActual;
        }
        set
        {
            _vidaActual = value;
        }
    }
    public int Levels
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }
    void Start()
    {
        health = Variables.Ghealth;
        _vidaActual = Variables.Gvida;
        exp = Variables.Gexp;
        level = Variables.Glvl;
        damage = Variables.Gdamage;
        atkspeed = Variables.Gatkspeed;
        stat = Variables.Gstat;
    }
    void Update()
    {
        if (exp == 100)
        {
            level++;
            _levelUp.activar(true);
            exp = 0;
            Variables.random1 = Random.Range(1, 4);
            Variables.random2 = Random.Range(1, 4);
            stat++;
        }
        if (stat > 0)
        {
            stats.text = "" + stat;
            levelUpTxt.enabled = true;
        }
        else
        {
            stats.text = "";
            levelUpTxt.enabled = false;
        }

        levelsT.text = "Lvl: " + level;
    }
}