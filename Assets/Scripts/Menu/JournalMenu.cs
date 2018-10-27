using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
//=========================================================================================
public class JournalMenu : MonoBehaviour {
    private Dictionary<string, Button> _uiElements = new Dictionary<string, Button>();
    public Sprite tickSprite;
    public Sprite dropDownArrowSprite;

    public Font font;

    private Transform _misionesActivasContainer;
    private Text _infoGralTxt;

    private const float OBJETIVOS_OFFSET = 10f;
    private const float TICK_OFFSET = 8f;
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

        addToJournal();
        updateObjectives();
        setMisionsAsCompleted();

        arangeUIElements();
    }
    //-------------------------------------------------------------------------------------
    void OnEnable(){
        updateUI();
    }
    //-------------------------------------------------------------------------------------
    void addToJournal() {
        List<Mision> misionesActivas = MisionesManager.getInstance().getMisionesEnCurso();

        if (misionesActivas != null && misionesActivas.Count > 0)
        {
            _infoGralTxt.text = misionesActivas[0].informacionGeneral;

            for (int i = 0; i < misionesActivas.Count; i++)
            {
                if (_uiElements.ContainsKey(misionesActivas[i].nombre))
                    continue;

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
                
                Button misionBtn = misionBtnObj.AddComponent<Button>();
                misionBtn.onClick.AddListener(mostrarInfoGral);
                
                _uiElements.Add(misionesActivas[i].nombre, misionBtn);

                GameObject completedTxtObj = new GameObject("completedTxt");
                completedTxtObj.transform.parent = misionBtnObj.transform;

                RectTransform completedTxtTransform = completedTxtObj.AddComponent<RectTransform>();
                completedTxtTransform.sizeDelta = new Vector2(129f, 25f);
                completedTxtTransform.localScale = new Vector2(1f, 1f);
                completedTxtTransform.localRotation = Quaternion.Euler(0f, 0f, 24f);
                completedTxtTransform.localPosition = new Vector2(15f, -50f);
                completedTxtTransform.pivot = new Vector2(0f, 0f);

                Text completedTxt = completedTxtObj.AddComponent<Text>();
                completedTxt.text = "Completed";
                completedTxt.color = new Color32(162, 35, 35, 255);
                completedTxt.alignment = TextAnchor.MiddleCenter;
                completedTxt.font = font;
                completedTxt.fontSize = 25;
                completedTxt.fontStyle = FontStyle.BoldAndItalic;
                completedTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
                completedTxt.verticalOverflow = VerticalWrapMode.Overflow;
                completedTxtObj.SetActive(false);

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

                for (int j = 0; j < misionesActivas[i].objetivos.Count; j++)
                {
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

                    GameObject tickObj = new GameObject("tick");
                    tickObj.transform.parent = objetivoTxtObj.transform;

                    RectTransform tickObjTransform = tickObj.AddComponent<RectTransform>();
                    tickObjTransform.localPosition = new Vector2(misionesActivas[i].objetivos[j].informacion.Length * TICK_OFFSET, 0);
                    tickObjTransform.pivot = new Vector2(0f, 0f);
                    tickObjTransform.sizeDelta = new Vector2(20f, 20f);
                    tickObjTransform.localScale = new Vector2(1f, 1f);

                    Image tickImg = tickObj.AddComponent<Image>();
                    tickImg.sprite = tickSprite;

                    tickObj.SetActive(false);
                }
            }
        }
    }
    //-------------------------------------------------------------------------------------
    void updateObjectives() {
        List<Mision> misionesActivas = MisionesManager.getInstance().getMisionesEnCurso();

        for (int i = 0; i < misionesActivas.Count; i++){
            GameObject misionBtnObj = _uiElements[misionesActivas[i].nombre].gameObject;

            for (int j = 0; j < misionesActivas[i].objetivos.Count; j++){
                Objetivo objetivo = misionesActivas[i].objetivos[j];

                if (objetivo.terminado) {
                    misionBtnObj.transform.GetChild(j + 2).GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }
    //-------------------------------------------------------------------------------------
    void arangeUIElements() {
        for (int i = 0; i < _uiElements.Count; i++){
            if (i == 0)
                _uiElements[_uiElements.ElementAt(i).Key].transform.localPosition = new Vector2(9f, -36 - (18 * i));
            else{
                float yPos = -36 - (18 * (_uiElements[_uiElements.ElementAt(i - 1).Key].transform.childCount));
                _uiElements[_uiElements.ElementAt(i).Key].transform.localPosition = new Vector2(9f, yPos);
            }
        }
    }
    //-------------------------------------------------------------------------------------
    void setMisionsAsCompleted() {
        List<Mision> misionesTerminadas = MisionesManager.getInstance().getMisionesTerminadas();

        for (int i = 0; i < misionesTerminadas.Count; i++){
            GameObject misionBtnObj = _uiElements[misionesTerminadas[i].nombre].gameObject;
            misionBtnObj.transform.GetChild(misionBtnObj.transform.childCount - 1).GetChild(0).gameObject.SetActive(true);
            misionBtnObj.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    //-------------------------------------------------------------------------------------
}
//=========================================================================================
