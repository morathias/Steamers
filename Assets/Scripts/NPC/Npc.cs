using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//===================================================================================================
[System.Serializable]
public class Dialogo
{
    [TextArea(4, 3)]
    public List<string> lineas;
    public int dialogoInteractivo;
    public List<bool> sonLineasInteractivas;
}
//===================================================================================================
public class Npc : MonoBehaviour
{
    private Image _dialogoBoxImg;
    private Text _mensajeTxt;
    private Text _accionTxt;
    private Animator NpcAni;

    public List<Mision> misiones;

    public List<Dialogo> dialogos;

    GameObject _prota;
    DialogoBox _dialogoBox;

    int _dialogoIndex;

    GameObject _activeMissionIcon;

    public string[] _misionesAActivar;

    enum estados
    {
        esperando,
        hablando,
        moviendose,
        cubriendose
    }
    estados _estado;
    //-----------------------------------------------------------------------------------------------
    void Start()
    {
        _estado = estados.esperando;
        _prota = GameObject.Find("Prota");
        _dialogoBoxImg = transform.Find("Dialogos").Find("dialogue_box").GetComponent<Image>();
        _mensajeTxt = _dialogoBoxImg.gameObject.transform.Find("linea").GetComponent<Text>();
        _accionTxt = transform.Find("Dialogos").Find("Accion").GetComponent<Text>();

        _dialogoBox = GetComponentInChildren<DialogoBox>();

        _dialogoBox.setInicioDialogo(dialogos[_dialogoIndex].lineas.ToArray(), dialogos[_dialogoIndex].dialogoInteractivo);
        NpcAni = transform.GetChild(0).GetComponent<Animator>();
        _activeMissionIcon = transform.GetChild(2).gameObject;

        if (misiones.Count == 0)
        {
            hideMisionIcon();
        }
        else
        {
            misiones[0].setUp();
            if (!misiones[0].enEspera)
                hideMisionIcon();
        }
    }
    //-----------------------------------------------------------------------------------------------
    void Update()
    {

        switch (_estado)
        {
            case estados.esperando:
                if (Vector3.Distance(gameObject.transform.position, _prota.transform.position) < 3)
                {
                    NpcAni.Play("Armature|Iddle");
                    _accionTxt.enabled = true;
                    _accionTxt.text = "E to talk";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        NpcAni.Play("Armature|Speaking");
                        _prota.GetComponent<Prota>().estaHablando(true);
                        _dialogoBoxImg.enabled = true;
                        _mensajeTxt.enabled = true;
                        _estado = estados.hablando;
                    }
                }

                else
                {
                    if (_accionTxt.enabled)
                        _accionTxt.enabled = false;
                }
                break;

            case estados.hablando:
                _accionTxt.text = "E: next";
                hideMisionIcon();
                if (_dialogoBox.finDialogo())
                {
                    _estado = estados.esperando;
                    _prota.GetComponent<Prota>().terminoDeHablar();

                    for (int i = 0; i < misiones.Count; i++)
                    {
                        if (misiones[i].getActiva())
                        {
                            showMisionIcon();
                            break;
                        }
                        else
                            hideMisionIcon();
                    }
                    
                }
                break;

            case estados.moviendose:
                break;
            case estados.cubriendose:
                break;
        }

        if (misiones.Count > 0)
        {
            misiones[0].Update();
        }
    }
    //-----------------------------------------------------------------------------------------------
    public bool estaHablando()
    {
        if (_estado == estados.hablando)
            return true;
        else
            return false;
    }
    //-----------------------------------------------------------------------------------------------
    public void setDialogo(int index)
    {
        _dialogoIndex = index;
        _dialogoBox.setInicioDialogo(dialogos[_dialogoIndex].lineas.ToArray(), dialogos[_dialogoIndex].dialogoInteractivo);
    }
    //-----------------------------------------------------------------------------------------------
    public void showMisionIcon() {
        if(!_activeMissionIcon.activeInHierarchy)
            _activeMissionIcon.SetActive(true);
    }

    public void hideMisionIcon() {
        _activeMissionIcon.SetActive(false);
    }
    //-----------------------------------------------------------------------------------------------
    public void empezarMisionEnEspera() {
        misiones[0].empezarMision();
    }
    //-----------------------------------------------------------------------------------------------
}
//===================================================================================================
