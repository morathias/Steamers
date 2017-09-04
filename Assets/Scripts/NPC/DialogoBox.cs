using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//================================================================================
public class DialogoBox : MonoBehaviour
{
    string[] _lineas;

    Text _texto;
    Image _dialogoBox;
    List<GameObject> _botones;

    int _indexDialogo = 0;
    int _finDialogo;
    int _dialogoInteractivo;
    //----------------------------------------------------------------------------
    void Start()
    {
        _dialogoBox = GetComponent<Image>();
        _texto = transform.GetChild(0).GetComponent<Text>();
        _botones = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Button>() != null)
                _botones.Add(transform.GetChild(i).gameObject);
        }
    }
    //----------------------------------------------------------------------------
    void Update()
    {
        if (_indexDialogo < _lineas.Length)
            _texto.text = _lineas[_indexDialogo];

        if (_indexDialogo == _dialogoInteractivo)
        {
            for (int i = 0; i < _botones.Count; i++)
                _botones[i].SetActive(true);
        }
        else
        {
            for (int i = 0; i < _botones.Count; i++)
                _botones[i].SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && _dialogoBox.enabled && _indexDialogo != _dialogoInteractivo)
            proximaLinea();
    }
    //----------------------------------------------------------------------------
    public void setInicioDialogo(string[] lineas, int dialogoInteractivo)
    {
        _indexDialogo = 0;
        _lineas = lineas;
        _dialogoInteractivo = dialogoInteractivo;
        _finDialogo = _lineas.Length;
        _texto.text = _lineas[_indexDialogo];
    }
    public void setFinDialogo(int fin)
    {
        _finDialogo = fin;
        Debug.Log(_finDialogo);
    }
    //----------------------------------------------------------------------------
    public bool finDialogo()
    {
        if (_finDialogo == _indexDialogo)
        {
            _dialogoBox.enabled = false;
            _texto.enabled = false;
            _indexDialogo = 0;
            return true;
        }
        else
            return false;
    }
    //----------------------------------------------------------------------------
    public void proximaLinea()
    {
        _indexDialogo++;
    }
}
//================================================================================
