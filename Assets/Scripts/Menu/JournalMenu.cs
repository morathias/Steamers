using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//=========================================================================================
public class JournalMenu : MonoBehaviour {
    private List<Graphic> _uiElements;
    public Sprite tickSprite;
    public Sprite dropDownArrowSprite;

    public Font font;

    private Transform _misionesActivasContainer;
    private Text _infoGralTxt;

    private const float OBJETIVOS_OFFSET = 10f;
    private int _ultimaMisionIndex = 0;
    //-------------------------------------------------------------------------------------
    public void mostrarInfoGral(){
        GameObject botonSeleccionado = EventSystem.current.currentSelectedGameObject;
        ObjetivoGUI objetivoBtn = botonSeleccionado.GetComponent<ObjetivoGUI>();

        _infoGralTxt.text = objetivoBtn.infoGral;
    }
    //-------------------------------------------------------------------------------------
    private void updateUI() {
        if(_misionesActivasContainer == null)
            _misionesActivasContainer = transform.Find("misionesActivas").Find("Viewport").Find("Content").transform;

        if(_infoGralTxt == null)
            _infoGralTxt = transform.Find("infoGral").Find("Viewport").Find("Content").Find("infoGralTxt").GetComponent<Text>();

        List<Mision> misionesActivas = MisionesManager.getInstance().getMisionesEnCurso();

        if (misionesActivas != null || misionesActivas.Count > 0) {
            _infoGralTxt.text = misionesActivas[0].informacionGeneral;

            for (int i = _ultimaMisionIndex; i < misionesActivas.Count; i++) {
                GameObject misionBtnObj = new GameObject(misionesActivas[i].nombre);
                misionBtnObj.transform.parent = _misionesActivasContainer;
                ObjetivoGUI objetivoGUI = misionBtnObj.AddComponent<ObjetivoGUI>();
                objetivoGUI.infoGral = misionesActivas[i].informacionGeneral;
                RectTransform misionBtnTransform = misionBtnObj.AddComponent<RectTransform>();
                misionBtnTransform.sizeDelta = new Vector2(300f, 18f);
                misionBtnTransform.localScale = new Vector2(1f, 1f);
                misionBtnTransform.anchorMax = new Vector2(0f, 1f);
                misionBtnTransform.anchorMin = new Vector2(0f, 1f);
                misionBtnTransform.pivot = new Vector2(0f, 0f);
                if (i == 0)
                    misionBtnTransform.localPosition = new Vector2(0, -36 - (18 * i));
                else
                    misionBtnTransform.localPosition = new Vector2(0, -36 - (18 * (misionesActivas[i - 1].objetivos.Count + 1)));
                Button misionBtn = misionBtnObj.AddComponent<Button>();
                misionBtn.onClick.AddListener(mostrarInfoGral);

                GameObject misionNombreTxtObj = new GameObject("nombreMisionTxt");
                misionNombreTxtObj.transform.parent = misionBtnObj.transform;
                RectTransform misionNombreTxtTransform = misionNombreTxtObj.AddComponent<RectTransform>();
                misionNombreTxtTransform.sizeDelta = new Vector2(300f, 18f);
                misionNombreTxtTransform.localScale = new Vector2(1f, 1f);
                misionNombreTxtTransform.anchorMax = new Vector2(0f, 1f);
                misionNombreTxtTransform.anchorMin = new Vector2(0f, 1f);
                misionNombreTxtTransform.pivot = new Vector2(0f, 0f);
                misionNombreTxtTransform.localPosition = new Vector2(0, 0);
                Text misionNombreTxt = misionNombreTxtObj.AddComponent<Text>();
                misionNombreTxt.text = misionesActivas[i].nombre;
                misionNombreTxt.font = font;
                misionNombreTxt.color = new Color32(50, 50, 50, 255);

                for (int j = 0; j < misionesActivas[i].objetivos.Count; j++) {
                    GameObject objetivoTxtObj = new GameObject("objectivo_" + misionesActivas[i].objetivos[j].informacion);
                    objetivoTxtObj.transform.parent = misionBtnObj.transform;
                    RectTransform objetivoTxtTransform = objetivoTxtObj.AddComponent<RectTransform>();
                    objetivoTxtTransform.sizeDelta = new Vector2(300, 18);
                    objetivoTxtTransform.localScale = new Vector2(1f, 1f);
                    objetivoTxtTransform.anchorMax = new Vector2(0f, 1f);
                    objetivoTxtTransform.anchorMin = new Vector2(0f, 1f);
                    objetivoTxtTransform.pivot = new Vector2(0f, 0f);
                    objetivoTxtTransform.localPosition = new Vector2(OBJETIVOS_OFFSET,
                                                                -18 - objetivoTxtTransform.sizeDelta.y * j);
                    Text objetivoTxt = objetivoTxtObj.AddComponent<Text>();
                    objetivoTxt.text = "-" + misionesActivas[i].objetivos[j].informacion;
                    objetivoTxt.font = font;
                    objetivoTxt.color = new Color32(50, 50, 50, 255);
                }

                _ultimaMisionIndex = i + 1;
            }
        }
    }
    //-------------------------------------------------------------------------------------
    void OnEnable(){
        updateUI();
    }
}
//=========================================================================================
