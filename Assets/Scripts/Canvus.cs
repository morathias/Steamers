using UnityEngine;
using System.Collections;

public class Canvus : MonoBehaviour {
     public Canvas Canvazorg;

	void Update () {
         if(Input.GetKeyDown (KeyCode.C) && Variables.limitador == 1){
             if (Canvazorg.enabled)
                 Canvazorg.enabled = false;
             else
                 Canvazorg.enabled = true;

         }
         if (Variables.limitador == 0)
             Canvazorg.enabled = false;
     }
 }

