using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HablarNpc : Objetivo
{
    public Npc npcAHablar;

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

            return true;
        }
        else
            return false;
    }
}
