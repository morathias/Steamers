using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//===================================================================================================
[System.Serializable]
public class Dialogo
{
    [TextArea(4, 3)]
    public string[] lineas;
    public int dialogoInteractivo;
}
//===================================================================================================
public class Npc : MonoBehaviour
{
    [Header("Viñeta Dialogo")]
    public Image dialogoBox;
    public Text mensaje;
    public Text accion;

    [Space(10.0f)]
    public Mision[] misiones;

    [Space(10.0f)]
    public Dialogo[] dialogos;

    GameObject _prota;
    DialogoBox _dialogoBox;

    int _dialogoIndex;

    Animator _animations;

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

        _dialogoBox.setInicioDialogo(dialogos[_dialogoIndex].lineas, dialogos[_dialogoIndex].dialogoInteractivo);

        _animations = transform.GetChild(0).GetComponent<Animator>();
    }
    //-----------------------------------------------------------------------------------------------
    void Update()
    {
        switch (_estado)
        {
            case estados.esperando:
                _animations.Play("Armature|Iddle");
                if (Vector3.Distance(gameObject.transform.position, _prota.transform.position) < 3)
                {
                    accion.enabled = true;
                    accion.text = "E to talk";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Debug.Log("dialogo index: " + _dialogoIndex);
                        _prota.GetComponent<Prota>().estaHablando(true);
                        dialogoBox.enabled = true;
                        mensaje.enabled = true;
                        _animations.Play("Armature|Speaking");
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

                if (_dialogoBox.finDialogo())
                {
                    Debug.Log("findialogo");
                    _estado = estados.esperando;
                    _prota.GetComponent<Prota>().terminoDeHablar();
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
        _dialogoBox.setInicioDialogo(dialogos[_dialogoIndex].lineas, dialogos[_dialogoIndex].dialogoInteractivo);
        Debug.Log("dialogo index: " + _dialogoIndex);
    }
}
//===================================================================================================
