using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour{
    enum Options{
        General,
        Graphics,
        Sound,
        LoadGame
    }
    Options _option = Options.General;

    public Canvas[] menues;
    public Canvas navigator;
    public GameObject cerrarBtn;

    public void activarOpciones(){
        menues[(int)Options.General].enabled = true;
        menues[(int)Options.Graphics].enabled = false;
        menues[(int)Options.Sound].enabled = false;
        menues[(int)Options.LoadGame].enabled = false;
        navigator.enabled = true;

        cerrarBtn.SetActive(true);
    }

    public void cerrarOpciones(){
        menues[(int)Options.General].enabled = false;
        menues[(int)Options.Graphics].enabled = false;
        menues[(int)Options.Sound].enabled = false;
        menues[(int)Options.LoadGame].enabled = false;
        navigator.enabled = false;

        cerrarBtn.SetActive(false);
    }

    void updateMenues(){
        switch (_option){
            case Options.General:
                menues[(int)Options.General].enabled = true;
                menues[(int)Options.Graphics].enabled = false;
                menues[(int)Options.Sound].enabled = false;
                menues[(int)Options.LoadGame].enabled = false;
                break;

            case Options.Graphics:
                menues[(int)Options.General].enabled = false;
                menues[(int)Options.Graphics].enabled = true;
                menues[(int)Options.Sound].enabled = false;
                menues[(int)Options.LoadGame].enabled = false;
                break;

            case Options.Sound:
                menues[(int)Options.General].enabled = false;
                menues[(int)Options.Graphics].enabled = false;
                menues[(int)Options.Sound].enabled = true;
                menues[(int)Options.LoadGame].enabled = false;
                break;

            case Options.LoadGame:
                menues[(int)Options.General].enabled = false;
                menues[(int)Options.Graphics].enabled = false;
                menues[(int)Options.Sound].enabled = false;
                menues[(int)Options.LoadGame].enabled = true;

                navigator.enabled = false;
                break;
        }
    }

    public void switchMenu(int option) {
        _option = (Options)option;
        updateMenues();

        cerrarBtn.SetActive(true);
    }
}