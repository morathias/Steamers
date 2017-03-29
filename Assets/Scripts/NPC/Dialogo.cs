using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//================================================================================
public class Dialogo : MonoBehaviour {
    [TextArea]
    public string[] lineas;

    public Text texto;
    public Image dialogoBox;
    List<GameObject> _botones;

    int _indexDialogo = 0;
    int _finDialogo;
    public int dialogoInteractivo;
    //----------------------------------------------------------------------------
	void Start () {
        texto.text = lineas[_indexDialogo];
        _botones = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Button>() != null)
                _botones.Add(transform.GetChild(i).gameObject);
        }
	}
    //----------------------------------------------------------------------------
	void Update () {
        if (_indexDialogo < lineas.Length)
            texto.text = lineas[_indexDialogo];

        if (_indexDialogo == dialogoInteractivo) {
            for (int i = 0; i < _botones.Count; i++)
                _botones[i].SetActive(true);
        }
        else{
            for (int i = 0; i < _botones.Count; i++)
                _botones[i].SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && dialogoBox.enabled && _indexDialogo != dialogoInteractivo)
            proximaLinea();
	}
    //----------------------------------------------------------------------------
    public void setInicioDialogo(int inicio) {
        _indexDialogo = inicio;
        Debug.Log("indexDialogo " + _indexDialogo);
    }
    public void setFinDialogo(int fin) {
        _finDialogo = fin;
        Debug.Log(_finDialogo);
    }
    //----------------------------------------------------------------------------
    public bool finDialogo() {
        if (_finDialogo + 1 == _indexDialogo)
        {
            dialogoBox.enabled = false;
            texto.enabled = false;
            return true;
        }
        else
            return false;
    }
    //----------------------------------------------------------------------------
    public void proximaLinea() {
        _indexDialogo++;
    }
}
//================================================================================
