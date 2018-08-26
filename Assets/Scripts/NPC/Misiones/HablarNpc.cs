using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HablarNpc : Objetivo
{
    [HideInInspector]
    public Npc npcAHablar;
    [HideInInspector]
    public int dialogoIndex;

    public override bool condicionCumplida()
    {
        if (npcAHablar.estaHablando())
        {
            Debug.Log("hablaste");
            Debug.Log("dialogo index: " + dialogoIndex);
            npcAHablar.setDialogo(dialogoIndex);

            _activo = false;
            _terminado = true;
            npcAHablar.hideMisionIcon();
            return true;
        }
        else
        {
            npcAHablar.showMisionIcon();
            return false;
        }
    }

    public override object downCast()
    {
        return this;
    }
}
