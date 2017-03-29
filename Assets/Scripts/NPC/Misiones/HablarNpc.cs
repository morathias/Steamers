using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HablarNpc : Objetivo {
    public Npc npcAHablar;

    public int inicioDialogo, finDialogo;

    public override bool condicionCumplida(){
        if (npcAHablar.estaHablando()){
            Debug.Log("hablaste");
            npcAHablar.inicioDialogo = inicioDialogo;
            npcAHablar.finDialogo = finDialogo;
            npcAHablar.dialogoBox.GetComponent<Dialogo>().setFinDialogo(finDialogo);

            _activo = false;
            _terminado = true;

            return true;
        }
        else
            return false;
    }
}
