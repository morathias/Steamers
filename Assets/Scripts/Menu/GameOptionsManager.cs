using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//=======================================================
public class GameOptionsManager : MonoBehaviour {
    enum Options {
        journal,
        stats,
        options,
        inventory
    }

    Options _options = Options.journal;

    public Canvas[] menues;

    MenuPausa _menuPausa;
	//----------------------------------------------------
    void Start() {
        _menuPausa = GameObject.Find("gameMaster").GetComponent<MenuPausa>();
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.J))
            switchMenu(0);
        /*if (Input.GetKeyDown(KeyCode.U))
            switchMenu(1);
        if (Input.GetKeyDown(KeyCode.O))
            switchMenu(2);
        if (Input.GetKeyDown(KeyCode.I))
            switchMenu(3);*/

        if (Input.GetKeyDown(KeyCode.Escape) && !_menuPausa.enabled) {
            _menuPausa.enabled = true;
            disableAll();
            Time.timeScale = 1;
        }
	}
    //----------------------------------------------------
    void updateMenues() {
        switch (_options)
        {
            case Options.journal:
                    enableJournalMenu();
                break;

            /*case Options.stats:
                enableStatsMenu();
                break;

            case Options.options:
                enableOptionsMenu();
                break;

            case Options.inventory:
                enableInventoryMenu();
                break;*/

            default:
                break;
        }
    }
    //----------------------------------------------------
    public void switchMenu(int index) {
        if (menues[index].enabled){
            menues[index].enabled = false;
            Time.timeScale = 1;
            return;
        }
        Time.timeScale = 0;
        _options = (Options)index;
        _menuPausa.enabled = false;
        updateMenues();
    }
    //----------------------------------------------------
    void enableJournalMenu() {
        menues[(int)Options.journal].enabled = true;
      //menues[(int)Options.stats].enabled = false;
      //menues[(int)Options.options].enabled = false;
      //menues[(int)Options.inventory].enabled = false;
    }
    //----------------------------------------------------
    /*void enableStatsMenu() {
        menues[(int)Options.journal].enabled = false;
        menues[(int)Options.stats].enabled = true;
        menues[(int)Options.options].enabled = false;
        menues[(int)Options.inventory].enabled = false;
    }*/
    //----------------------------------------------------
    /*void enableOptionsMenu() {
        menues[(int)Options.journal].enabled = false;
        menues[(int)Options.stats].enabled = false;
        menues[(int)Options.options].enabled = true;
        menues[(int)Options.inventory].enabled = false;
    }*/
    //----------------------------------------------------
    /*void enableInventoryMenu() {
        menues[(int)Options.journal].enabled = false;
        menues[(int)Options.stats].enabled = false;
        menues[(int)Options.options].enabled = false;
        menues[(int)Options.inventory].enabled = true;
    }*/
    //----------------------------------------------------
    void disableAll() {
        menues[(int)Options.journal].enabled = false;
        //menues[(int)Options.stats].enabled = false;
        //menues[(int)Options.options].enabled = false;
        //menues[(int)Options.inventory].enabled = false;
    }
}
//=======================================================
