using UnityEngine;
using System.Collections;

public class Canvus : MonoBehaviour {
     public Canvas Canvazorg;

     void Start(){
     }
	void Update () {
         if(Input.GetKeyDown (KeyCode.C)){
             if (Canvazorg.enabled)
                 Canvazorg.enabled = false;
             else
                 Canvazorg.enabled = true;

         }
     }
 }

