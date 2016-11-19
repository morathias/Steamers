using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//================================================================================
public class Dialogo : MonoBehaviour {
    public string[] lineas;
    public Text texto;
    public Image dialogoBox;

    int _indexDialogo = 0;
    //----------------------------------------------------------------------------
	void Start () {
        lineas[_indexDialogo] = lineas[_indexDialogo].Replace("/", "\n");
        texto.text = lineas[_indexDialogo];
	}
    //----------------------------------------------------------------------------
	void Update () {
        if (_indexDialogo < lineas.Length)
        {
            texto.text = lineas[_indexDialogo];
            lineas[_indexDialogo] = lineas[_indexDialogo].Replace("/", "\n");
        }
        else
        {
            dialogoBox.enabled = false;
            texto.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && dialogoBox.enabled)
            _indexDialogo++;
	}
    //----------------------------------------------------------------------------
    public void setInicioDialogo(int inicio) {
        _indexDialogo = inicio;
    }
    //----------------------------------------------------------------------------
    public bool finDialogo() {
        if (_indexDialogo == lineas.Length)
            return true;
        else
            return false;
    }
    //----------------------------------------------------------------------------
}
//================================================================================
