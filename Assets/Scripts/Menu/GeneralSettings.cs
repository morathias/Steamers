using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSettings : MonoBehaviour {
    Resolution[] _resolutions;

    public Toggle pantallaCompletaBox;
    public Dropdown resoluciones;

    void Start() {
        pantallaCompletaBox.isOn = true;
        _resolutions = new Resolution[17];
        setResolutions();
    }

    void setResolutions() {
        _resolutions[0].width = 640;
        _resolutions[0].height = 480;

        _resolutions[1].width = 720;
        _resolutions[1].height = 480;

        _resolutions[2].width = 800;
        _resolutions[2].height = 600;

        _resolutions[3].width = 1024;
        _resolutions[3].height = 768;

        _resolutions[4].width = 1152;
        _resolutions[4].height = 864;

        _resolutions[5].width = 1280;
        _resolutions[5].height = 720;

        _resolutions[6].width = 1280;
        _resolutions[6].height = 768;

        _resolutions[7].width = 1280;
        _resolutions[7].height = 800;

        _resolutions[8].width = 1280;
        _resolutions[8].height = 960;

        _resolutions[9].width = 1280;
        _resolutions[9].height = 1024;

        _resolutions[10].width = 1360;
        _resolutions[10].height = 768;

        _resolutions[11].width = 1360;
        _resolutions[11].height = 1024;

        _resolutions[12].width = 1366;
        _resolutions[12].height = 768;

        _resolutions[13].width = 1400;
        _resolutions[13].height = 1050;

        _resolutions[14].width = 1440;
        _resolutions[14].height = 900;

        _resolutions[15].width = 1600;
        _resolutions[15].height = 900;

        _resolutions[16].width = 1600;
        _resolutions[16].height = 1200;
    }

	public void cambiarPantallaCompleta () {
        Screen.fullScreen = pantallaCompletaBox.isOn;
	}

    public void setScreenResolution() {
        Screen.SetResolution(_resolutions[resoluciones.value].width, _resolutions[resoluciones.value].height, pantallaCompletaBox.isOn);
    }

}
