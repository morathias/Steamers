using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public float health = Variables.Ghealth;
    private float _vidaActual;
    public float exp;
    public int healthPool = 100;
    private int level;
    public float damage;
    public float damageFinal;
    public float damageBase = 10;
    public int stat;
    public float buffReload = 1.5f;
    public Text stats;
    public Text levelsT;
    public Text levelUpTxt;
    public LevelUp _levelUp;
    public bool RageOn = false;
    public float regen = 0.3f;
    public int rage = 0;
    public bool onKilling = false;

    public float timerRage = 0f;

    public bool applyDamage(int damage)
    {
        _vidaActual -= damage;
        if (_vidaActual <= 0)
            return true;
        return false;
    }

    public float VidaActual
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
        stat = Variables.Gstat;
        damageFinal = damage + rage * 0.5f;
    }
    void Update()
    {
        damageFinal = damage + rage * 0.5f;
        if (rage > 0)
        {
            timerRage += Time.deltaTime;
            if (timerRage > 5)
            {
                rage = 0;
                timerRage = 0;
            }
        }
        if (exp > 100*(1+level*0.5))
        {
            level++;
            _levelUp.activar(true);
            exp = exp-(100 * (1 + level * 0.5f));
            stat++;
        }
        if (stat > 0)
        {
            stats.text = "Points:" + stat;
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