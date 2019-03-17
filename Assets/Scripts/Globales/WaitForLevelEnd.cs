using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForLevelEnd : MonoBehaviour {
	public Npc finalMision;
	public EsceneManager sceneManager;
	bool _changingScene = false;
	// Update is called once per frame
	void Update () {
		if(finalMision.misiones[0].isEnded() && !_changingScene){
			_changingScene = true;
			sceneManager.cambiarEscena(2);
		}
	}
}
