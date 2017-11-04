using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkillTree : MonoBehaviour {

    private Stats statsComponent;
    private Prota protaComponent;
    private Arma armaComponent;
    private Pota potaComponent;
    public Button[] buttons;
    public int damageCounter = 0;
    public int staminaCounter = 0;
    public int healthCounter = 0;
    public float tenP = 0.1f;
    public float fiveP = 0.05f;
    void Start () {
        statsComponent = GameObject.Find("Prota").gameObject.GetComponent<Stats>();
        protaComponent = GameObject.Find("Prota").gameObject.GetComponent<Prota>();
        armaComponent = GameObject.Find("Prota").transform.GetChild(0).GetComponent<Arma>();
    }
	void Update () {}
    public void extraDamage(){
        if (statsComponent.stat > 0){
            statsComponent.damage += statsComponent.damageBase * tenP;
            statsComponent.stat--;
            damageCounter++;
            if (damageCounter == 3)
                buttons[1].interactable = true;
            if (damageCounter == 4)
                buttons[2].interactable = true;
            if (damageCounter == 5)
            {
                buttons[3].interactable = true;
                buttons[0].interactable = false;
            }
        }
    }
    public void extraStamina(){
        if (statsComponent.stat > 0){
            protaComponent.Stamina += protaComponent.staminaBase * fiveP;
            statsComponent.stat--;
            staminaCounter++;
            if (staminaCounter == 3)
                buttons[5].interactable = true;
            if (staminaCounter == 4)
                buttons[6].interactable = true;
            if (staminaCounter == 5)
            {
                buttons[7].interactable = true;
                buttons[4].interactable = false;
            }
        }
    }
    public void extraHealt() {
        if (statsComponent.stat > 0){
            statsComponent.health += statsComponent.healthPool * tenP;
            statsComponent.VidaActual += statsComponent.healthPool * tenP;
            statsComponent.stat--;
            healthCounter++;
            if (healthCounter == 3)
                buttons[9].interactable = true;
            if (healthCounter == 4)
                buttons[10].interactable = true;
            if (healthCounter == 5)
            {
                buttons[11].interactable = true;
                buttons[8].interactable = false;
            }
        }
    }
    public void extraMagazine(){
        if (statsComponent.stat > 0){
            armaComponent.balas += 16;
            statsComponent.stat--;
            buttons[1].interactable = false;
        }
    }
    public void reloadOp() {
        if (statsComponent.stat > 0){
            statsComponent.buffReload = 2;
            statsComponent.stat--;
            buttons[2].interactable = false;
        }
    }
    public void rageKill(){
        if (statsComponent.stat > 0){
            statsComponent.RageOn = true;
            statsComponent.stat--;
            buttons[3].interactable = false;
        }
    }
    public void shieldStamina(){
        if (statsComponent.stat > 0){
            protaComponent.consumoShield = 0.15f;
            statsComponent.stat--;
            buttons[6].interactable = false;
        }
    }
    public void LlamaStamina()
    {
        if (statsComponent.stat > 0){
            protaComponent.consumoLlama = 0.15f;
            statsComponent.stat--;
            buttons[5].interactable = false;
        }
    }
    public void DashStamina()
    {
        if (statsComponent.stat > 0){
            protaComponent.consumoDash = 20f;
            statsComponent.stat--;
            buttons[7].interactable = false;
        }
    }
    public void potaRegen(){
         if (statsComponent.stat > 0){
            statsComponent.regen = 0.5f;
            statsComponent.stat--;
            buttons[9].interactable = false;
         }
    }
    public void lifeOnKill(){
        if (statsComponent.stat > 0){
            statsComponent.onKilling = true;
            statsComponent.stat--;
            buttons[11].interactable = false;
        }
    }
    public void extraHewlettPackard() {
        if (statsComponent.stat > 0){
            statsComponent.health += healthCounter * statsComponent.healthPool * tenP;
            statsComponent.VidaActual += healthCounter * statsComponent.healthPool * tenP;
            statsComponent.stat--;
            buttons[10].interactable = false;
        }
    }
}
