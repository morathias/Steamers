using UnityEngine;
using System.Collections;
//===================================================================
public class MenuPausa : MonoBehaviour {
    Canvas menuPausa;
    Prota _prota;
    //---------------------------------------------------------------
	/*void Awake(){
        DontDestroyOnLoad(gameObject);
    }*/
    //---------------------------------------------------------------
	void Start () {
        _prota = GameObject.Find("Prota").GetComponent<Prota>();
        menuPausa = GameObject.Find("menuPausa").GetComponent<Canvas>();
	}
    //---------------------------------------------------------------
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pausa();
        }

        
	}
    //---------------------------------------------------------------
    public void pausa() {
        if (!menuPausa)
            menuPausa = GameObject.Find("menuPausa").GetComponent<Canvas>();

        if (!_prota)
            _prota = GameObject.Find("Prota").GetComponent<Prota>();

        if (menuPausa.enabled){
            menuPausa.enabled = false;
            Time.timeScale = 1;
            if(_prota)
                _prota.enabled = true;
        }

        else{
            menuPausa.enabled = true;
            Time.timeScale = 0;
            if(_prota)
                _prota.enabled = false;
        }
    }
    //---------------------------------------------------------------
}
//===================================================================
