using UnityEngine;
using System.Collections;

public class Canvus : MonoBehaviour {
     public Canvas Canvazorg;
     private Stats levelComponent;

     void Start(){
         levelComponent = GameObject.Find("Prota").gameObject.GetComponent<Stats>();
     }
	void Update () {
         if(Input.GetKeyDown (KeyCode.C) && levelComponent.stat >= 1){
             if (Canvazorg.enabled)
                 Canvazorg.enabled = false;
             else
                 Canvazorg.enabled = true;

         }
         if (levelComponent.stat == 0)
             Canvazorg.enabled = false;
     }
 }

