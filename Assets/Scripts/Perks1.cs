using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Perks1 : MonoBehaviour {
    public Text Perk1;
    private Stats levelComponent;
    public int randum;
    void Start(){
        levelComponent = GameObject.Find("Prota").gameObject.GetComponent<Stats>();
    }
   void Update()
    {
        switch (Variables.random1)
        {
            case 1:
                Perk1.text = "Extra Damage";
                break;
            case 2:
                Perk1.text = "Extra Health";
                break;
            case 3:
                Perk1.text = "Extra Speed";
                break;
            case 4:
                Perk1.text = "Extra AtkSpeed";
                break;
        }
    }
   public void perks()
   {
       levelComponent = GameObject.Find("Prota").gameObject.GetComponent<Stats>();
       Prota protaComponent = GameObject.Find("Prota").gameObject.GetComponent<Prota>();
       switch (Variables.random1)
       {
           case 1:
               levelComponent.damage += 2;
               Variables.choose = true;
               Variables.limitador = 0;
               break;
           case 2:
               levelComponent.health += 15;
               Variables.choose = true;
               Variables.limitador = 0;
               break;
           case 3:
               protaComponent.velocidad += 2;
               Variables.choose = true;
               Variables.limitador = 0;
               break;
           case 4:
               levelComponent.atkspeed += 2;
               Variables.choose = true;
               Variables.limitador = 0;
               break;
       }
    }
}
