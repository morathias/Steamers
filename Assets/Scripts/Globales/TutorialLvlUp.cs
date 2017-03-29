using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLvlUp : MonoBehaviour {
    public Text lvlUpTxt;
    public Stats lvl;
	
	void Update () {
        if (lvl.stat > 0)
            lvlUpTxt.enabled = true;

        if (lvlUpTxt.enabled && Input.GetKeyDown(KeyCode.C))
            Destroy(gameObject);
	}
}
