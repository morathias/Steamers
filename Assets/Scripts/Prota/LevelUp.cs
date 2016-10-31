using UnityEngine;
using System.Collections;

public class LevelUp : MonoBehaviour {

    public ParticleSystem levelUpParticle;
    public Light levelUpLight;

    float timer = 1.0f;
    bool activado;
	
	void Update () {
        if (activado) {
            timer -= Time.deltaTime;
            levelUpLight.enabled = true;
           
        }

        if(timer<=0){
            timer = 2.0f;
            activado = false;
            levelUpLight.enabled = false;
            levelUpParticle.Stop();
        }
	}

    public void activar(bool activar) {
        activado = true;
        levelUpParticle.Play();
    }
}
