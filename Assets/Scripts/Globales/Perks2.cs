using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Perks2 : MonoBehaviour{

    public Text Perk2;
    private Stats levelComponent;
    void Start()
    {
        levelComponent = GameObject.Find("Prota").gameObject.GetComponent<Stats>();
    }
    void Update()
    {
        switch (Variables.random2)
        {
            case 1:
                Perk2.text = "Extra Damage";
                break;
            case 2:
                Perk2.text = "Extra Health";
                break;
            case 3:
                Perk2.text = "Extra Speed";
                break;
            case 4:
                Perk2.text = "Extra AtkSpeed";
                break;
        }
    }
    public void perks(){
        levelComponent = GameObject.Find("Prota").gameObject.GetComponent<Stats>();
        Prota protaComponent = GameObject.Find("Prota").gameObject.GetComponent<Prota>();
        switch (Variables.random2){
            case 1:
                levelComponent.damage += 2;
                levelComponent.stat --;
                break;
            case 2:
                levelComponent.health += 15;
                levelComponent.vida += 15;
                levelComponent.stat--;
                break;
            case 3:
                protaComponent.velocidad += 2;
                levelComponent.stat--;
                break;
            case 4:
                levelComponent.atkspeed += 2;
                levelComponent.stat--;
                break;
        }
        /*Debug.Log("Damage: " + levelComponent.damage);
         Debug.Log("Health: " + levelComponent.health);
         Debug.Log("HealthActual: " + levelComponent.vida);
         Debug.Log("Speed: " + protaComponent.velocidad);
         Debug.Log("AtkSpeed: " + levelComponent.atkspeed);*/
    }
}
