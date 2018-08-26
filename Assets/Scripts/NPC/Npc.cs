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
    [Header("Viñeta Dialogo")]
    public Image dialogoBox;
    public Text mensaje;
    public Text accion;
    private Animator NpcAni;

    [Space(10.0f)]
    public List<Mision> misiones;

    [Space(10.0f)]
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
                    accion.enabled = true;
                    accion.text = "E to talk";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        NpcAni.Play("Armature|Speaking");
                        _prota.GetComponent<Prota>().estaHablando(true);
                        dialogoBox.enabled = true;
                        mensaje.enabled = true;
                        _estado = estados.hablando;
                    }
                }

                else
                {
                    if (accion.enabled)
                        accion.enabled = false;
                }
                break;

            case estados.hablando:
                accion.text = "E: next";
                hideMisionIcon();
                if (_dialogoBox.finDialogo())
                {
                    Debug.Log("findialogo");
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
        Debug.Log("dialogo index: " + _dialogoIndex);
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
