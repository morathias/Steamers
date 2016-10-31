using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//================================================================================
public class Dialogo : MonoBehaviour {
    public string[] lineas;
    public Text texto;
    public Canvas canvas;

    int _indexDialogo = 0;
    //----------------------------------------------------------------------------
	void Start () {
        texto.text = lineas[_indexDialogo];
	}
    //----------------------------------------------------------------------------
	void Update () {
        if (_indexDialogo < lineas.Length)
            texto.text = lineas[_indexDialogo];
        else
        {
            GameObject.Find("Prota").GetComponent<Prota>().estaHablando(false);
            canvas.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canvas.enabled)
            _indexDialogo++;
	}
    //----------------------------------------------------------------------------
    public void setInicioDialogo(int inicio) {
        _indexDialogo = inicio;
    }

    public bool finDialogo() {
        if (_indexDialogo == lineas.Length)
            return true;
        else
            return false;
    }
}
//================================================================================
