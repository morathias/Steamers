using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
//==========================================================================================================================
[CustomEditor(typeof(Npc))]
public class NpcEditor : Editor
{
    private const float MISIONES_BOTONES_WIDTH = 20;
    //----------------------------------------------------------------------------------------------------------------------
    public override void OnInspectorGUI()
    {
        Npc targetNpc = target as Npc;

        EditorGUILayout.LabelField("Misiones:");

        if (GUILayout.Button("Add"))
        {
            addMision(targetNpc);
        }

        for (int i = 0; i < targetNpc.misiones.Count; i++)
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical("HelpBox", GUILayout.Width(Screen.width - MISIONES_BOTONES_WIDTH * 3f));

            EditorGUIUtility.labelWidth = 70;
            targetNpc.misiones[i].nombre = EditorGUILayout.TextField("Nombre:", targetNpc.misiones[i].nombre);

            EditorGUILayout.LabelField("Informacion General:");
            targetNpc.misiones[i].informacionGeneral = EditorGUILayout.TextArea(targetNpc.misiones[i].informacionGeneral,
                                                                                GUILayout.Height(40),
                                                                                GUILayout.MaxHeight(50),
                                                                                GUILayout.MaxWidth(Screen.width - MISIONES_BOTONES_WIDTH * 3.3f));
            GUILayout.BeginHorizontal(GUILayout.Width(200));
            if (!targetNpc.misiones[i].isAlreadyActivated)
                targetNpc.misiones[i].enEspera = EditorGUILayout.Toggle("En Espera:", targetNpc.misiones[i].enEspera);

            EditorGUIUtility.labelWidth = 120;
            if (!targetNpc.misiones[i].enEspera)
                targetNpc.misiones[i].isAlreadyActivated = EditorGUILayout.Toggle("Ya Estaba Activada: ",
                                                                                  targetNpc.misiones[i].isAlreadyActivated);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(10));
            EditorGUILayout.LabelField("Recompensas:");

            if (targetNpc.misiones[i].recompensas == null)
                targetNpc.misiones[i].recompensas = new List<Item>();

            if (GUILayout.Button("+"))
                targetNpc.misiones[i].recompensas.Add(null);
            GUILayout.EndHorizontal();

            for (int j = 0; j < targetNpc.misiones[i].recompensas.Count; j++)
            {
                GUILayout.BeginHorizontal();
                targetNpc.misiones[i].recompensas[j] = EditorGUILayout.ObjectField(targetNpc.misiones[i].recompensas[j], typeof(Item), true) as Item;

                if (GUILayout.Button("-", GUILayout.Width(MISIONES_BOTONES_WIDTH)))
                {
                    targetNpc.misiones[i].recompensas.RemoveAt(j);
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Objetivos:");

            if (targetNpc.misiones[i].objetivos == null)
                targetNpc.misiones[i].objetivos = new List<Objetivo>();

            int cantidadTipos = (int)TiposObjetivos.CantidadTipos;
            for (int j = 0; j < cantidadTipos; j++)
            {
                if (j % 3 == 0) EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(((TiposObjetivos)j).ToString()))
                    agregarObjetivo((TiposObjetivos)j, targetNpc.misiones[i]);

                if (j % 3 == 2) EditorGUILayout.EndHorizontal();

                if (j == cantidadTipos - 1 && (cantidadTipos - 1) % 3 == 0 || 
                    j == cantidadTipos - 1 && (cantidadTipos - 1) % 3 != 2)
                    EditorGUILayout.EndHorizontal();
            }

            for (int j = 0; j < targetNpc.misiones[i].objetivos.Count; j++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical("Box", GUILayout.Width(Screen.width - MISIONES_BOTONES_WIDTH * 5f));
                EditorGUIUtility.labelWidth = 80;
                Objetivo objetivo = targetNpc.misiones[i].objetivos[j];
                EditorGUILayout.LabelField(objetivo.tipo.ToString());
                objetivo.informacion = EditorGUILayout.TextField("Informacion:", objetivo.informacion);

                agregarDataEspecificaObjetivo(targetNpc.misiones[i], objetivo);

                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                RemoveObjetivoButton(targetNpc.misiones[i], objetivo);
                moveUpObjetivoButton(targetNpc.misiones[i], objetivo);
                moveDownObjetivoButton(targetNpc.misiones[i], objetivo);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            moveUpMisionButton(targetNpc, targetNpc.misiones[i]);
            moveDownMisionButton(targetNpc, targetNpc.misiones[i]);
            removeMisionButton(targetNpc, targetNpc.misiones[i]);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Dialogos:");

        if (GUILayout.Button("Add"))
        {
            agregarDialogo(targetNpc);
        }

        for (int i = 0; i < targetNpc.dialogos.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("HelpBox", GUILayout.Width(Screen.width - MISIONES_BOTONES_WIDTH * 3f));

            EditorGUILayout.LabelField("Dialogo " + i);

            agregarLinea(targetNpc.dialogos[i]);

            if (targetNpc.dialogos[i].lineas == null)
                targetNpc.dialogos[i].lineas = new List<string>();

            Undo.RecordObject(target, "mision");
            EditorUtility.SetDirty(target);
            if (GUI.changed)
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            for (int j = 0; j < targetNpc.dialogos[i].lineas.Count; j++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.LabelField("Linea " + j);
                targetNpc.dialogos[i].lineas[j] = EditorGUILayout.TextArea(targetNpc.dialogos[i].lineas[j], GUILayout.Width(100), GUILayout.Height(70));

                EditorGUILayout.EndVertical();

                GUI.changed = false;
                targetNpc.dialogos[i].sonLineasInteractivas[j] = GUILayout.Toggle(targetNpc.dialogos[i].sonLineasInteractivas[j], "Activar dialogo Interactivo", "Button");

                if (targetNpc.dialogos[i].sonLineasInteractivas[j])
                    targetNpc.dialogos[i].dialogoInteractivo = j;

                if (GUI.changed && !targetNpc.dialogos[i].sonLineasInteractivas[j])
                    targetNpc.dialogos[i].dialogoInteractivo = -1;

                removeLineaButton(targetNpc.dialogos[i], targetNpc.dialogos[i].lineas[j]);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            GUILayout.BeginVertical();
            moveDownDialogoButton(targetNpc, targetNpc.dialogos[i]);
            moveUpDialogoButton(targetNpc, targetNpc.dialogos[i]);
            removeDialogoButton(targetNpc, targetNpc.dialogos[i]);
            GUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }
    }

    private void agregarLinea(Dialogo dialogo)
    {
        if (GUILayout.Button("Add line"))
        {
            dialogo.lineas.Add("");

            if (dialogo.sonLineasInteractivas == null)
                dialogo.sonLineasInteractivas = new List<bool>();
            dialogo.dialogoInteractivo = -1;
            dialogo.sonLineasInteractivas.Add(false);
        }
    }

    //----------------------------------------------------------------------------------------------------------------------
    private void removeLineaButton(Dialogo dialogo, string linea)
    {
        if (GUILayout.Button("-", GUILayout.Width(MISIONES_BOTONES_WIDTH)))
        {
            dialogo.sonLineasInteractivas.RemoveAt(dialogo.lineas.IndexOf(linea));
            dialogo.lineas.Remove(linea);
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void agregarDialogo(Npc targetNpc)
    { 
        targetNpc.dialogos.Add(new Dialogo());
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void removeDialogoButton(Npc targetNpc, Dialogo dialogo)
    {
        if (GUILayout.Button("-", GUILayout.Width(MISIONES_BOTONES_WIDTH)))
        {
            targetNpc.dialogos.Remove(dialogo);
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void moveDownDialogoButton(Npc targetNpc, Dialogo dialogo)
    {
        if (GUILayout.Button("v", GUILayout.Width(MISIONES_BOTONES_WIDTH)))
        {
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void moveUpDialogoButton(Npc targetNpc, Dialogo dialogo)
    {
        if (GUILayout.Button("^", GUILayout.Width(MISIONES_BOTONES_WIDTH)))
        {
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void removeMisionButton(Npc targetNpc, Mision mision)
    {
        if (GUILayout.Button("-", GUILayout.Width(MISIONES_BOTONES_WIDTH)))
        {
            targetNpc.misiones.Remove(mision);

            for (int i = 0; i < mision.objetivos.Count; i++)
            {
                DestroyImmediate(mision.objetivos[i].gameObject);
            }

            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void moveDownMisionButton(Npc targetNpc, Mision mision)
    {
        if (GUILayout.Button("v", GUILayout.Width(MISIONES_BOTONES_WIDTH)))
        {
            targetNpc.misiones.Insert(targetNpc.misiones.IndexOf(mision) + 1, mision);
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void moveUpMisionButton(Npc targetNpc, Mision mision)
    {
        if (GUILayout.Button("^", GUILayout.Width(MISIONES_BOTONES_WIDTH)))
        {
            targetNpc.misiones.Insert(targetNpc.misiones.IndexOf(mision) - 1, mision);
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void agregarObjetivo(TiposObjetivos tipo, Mision mision)
    {
        switch (tipo)
        {
            case TiposObjetivos.CazarEnemigos:
                GameObject objetivoCEObj = new GameObject(mision.nombre + "_cazar_enemigo");
                objetivoCEObj.transform.parent = (target as Npc).transform;
                //objetivoCEObj.SetActive(false);
                CazarEnemigos objetivoCE = objetivoCEObj.AddComponent<CazarEnemigos>();
                objetivoCE.tipo = tipo;

                mision.objetivos.Add(objetivoCE);
                break;

            case TiposObjetivos.CazarBoss:
                GameObject objetivoCBObj = new GameObject(mision.nombre + "_cazar_boss");
                objetivoCBObj.transform.parent = (target as Npc).transform;
                //objetivoCBObj.SetActive(false);
                CazarBoss objetivoCB = objetivoCBObj.AddComponent<CazarBoss>();
                objetivoCB.tipo = tipo;

                mision.objetivos.Add(objetivoCB);
                break;

            case TiposObjetivos.EncontrarItem:
                GameObject objetivoEIObj = new GameObject(mision.nombre + "_encontrar_item");
                objetivoEIObj.transform.parent = (target as Npc).transform;
                //objetivoEIObj.SetActive(false);
                EncontrarItem objetivoEI = objetivoEIObj.AddComponent<EncontrarItem>();
                objetivoEI.tipo = tipo;

                mision.objetivos.Add(objetivoEI);
                break;

            case TiposObjetivos.EscoltarObjetivo:
                GameObject objetivoEOObj = new GameObject(mision.nombre + "_escoltar_objetivo");
                objetivoEOObj.transform.parent = (target as Npc).transform;
                //objetivoEOObj.SetActive(false);
                EscoltarObjetivo objetivoEO = objetivoEOObj.AddComponent<EscoltarObjetivo>();
                objetivoEO.tipo = tipo;

                mision.objetivos.Add(objetivoEO);
                break;

            case TiposObjetivos.HablarBoss:
                GameObject objetivoHBObj = new GameObject(mision.nombre + "_hablar_Boss");
                objetivoHBObj.transform.parent = (target as Npc).transform;
                //objetivoHBObj.SetActive(false);
                HablarBoss objetivoHB = objetivoHBObj.AddComponent<HablarBoss>();
                objetivoHB.tipo = tipo;

                mision.objetivos.Add(objetivoHB);
                break;

            case TiposObjetivos.HablarNpc:
                GameObject objetivoHNObj = new GameObject(mision.nombre + "_hablar_npc");
                objetivoHNObj.transform.parent = (target as Npc).transform;
                //objetivoHNObj.SetActive(false);
                HablarNpc objetivoHN = objetivoHNObj.AddComponent<HablarNpc>();
                objetivoHN.tipo = tipo;

                mision.objetivos.Add(objetivoHN);
                break;

            case TiposObjetivos.IrAZona:
                GameObject objetivoIAZObj = new GameObject(mision.nombre + "_ir_a_zona");
                objetivoIAZObj.transform.parent = (target as Npc).transform;
                //objetivoIAZObj.SetActive(false);
                IrAZona objetivoIAZ = objetivoIAZObj.AddComponent<IrAZona>();
                objetivoIAZ.tipo = tipo;

                mision.objetivos.Add(objetivoIAZ);
                break;
        }

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void addMision(Npc targetNpc)
    {
        if (targetNpc.misiones == null)
            targetNpc.misiones = new List<Mision>();

        Mision misionAAgregar = new Mision();
        targetNpc.misiones.Add(misionAAgregar);
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void RemoveObjetivoButton(Mision mision, Objetivo objetivo)
    {
        if (GUILayout.Button("-", GUILayout.Width(MISIONES_BOTONES_WIDTH)))
        {
            mision.objetivos.Remove(objetivo);
            DestroyImmediate(objetivo.gameObject);

            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void moveDownObjetivoButton(Mision mision, Objetivo objetivo)
    {
        if (GUILayout.Button("v", GUILayout.Width(MISIONES_BOTONES_WIDTH)))
        {
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void moveUpObjetivoButton(Mision mision, Objetivo objetivo)
    {
        if (GUILayout.Button("^", GUILayout.Width(MISIONES_BOTONES_WIDTH)))
        {
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
    private void agregarDataEspecificaObjetivo(Mision mision, Objetivo objetivo)
    {
        switch (objetivo.tipo)
        {
            case TiposObjetivos.CazarEnemigos:
                CazarEnemigos objetivoCE = (CazarEnemigos)objetivo.downCast();

                if (objetivoCE == null)
                {
                    return;
                }

                GUI.changed = false;
                EditorGUIUtility.labelWidth = 110;
                objetivoCE.cantidadACazar = EditorGUILayout.IntField("Cantidad a cazar:", objetivoCE.cantidadACazar);
                EditorGUIUtility.labelWidth = 70;
                objetivoCE.esRandom = EditorGUILayout.Toggle("Es random:", objetivoCE.esRandom);
                if (GUI.changed)
                {
                    objetivoCE.tiposEnemigos.Clear();
                    for (int i = 0; i < objetivoCE.cantidadACazar; i++)
                        objetivoCE.tiposEnemigos.Add(null);
                }
                if (!objetivoCE.esRandom)
                {
                    EditorGUILayout.LabelField("Enemigos a cazar:");
                    for (int i = 0; i < objetivoCE.tiposEnemigos.Count; i++)
                    {
                        objetivoCE.tiposEnemigos[i] = (Overlord)EditorGUILayout.ObjectField(objetivoCE.tiposEnemigos[i], typeof(Overlord), true);
                    }
                }
                break;

            case TiposObjetivos.CazarBoss:
                break;

            case TiposObjetivos.EncontrarItem:
                EncontrarItem objetivoEI = (EncontrarItem)objetivo.downCast();

                EditorGUIUtility.labelWidth = 110;
                objetivoEI.cantidad = EditorGUILayout.IntField("Cantidad a agarrar:", objetivoEI.cantidad);
                EditorGUIUtility.labelWidth = 70;
                objetivoEI.esRandom = EditorGUILayout.Toggle("Es random:", objetivoEI.esRandom);

                if (!objetivoEI.esRandom)
                {
                    objetivoEI.itemAEncontrar = (Item.Tipo)EditorGUILayout.EnumPopup("Tipo de item a agarrar:", objetivoEI.itemAEncontrar);
                }

                break;

            case TiposObjetivos.EscoltarObjetivo:
                EscoltarObjetivo objetivoEO = (EscoltarObjetivo)objetivo.downCast();

                objetivoEO.camino = (GameObject)EditorGUILayout.ObjectField("Camino:", objetivoEO.camino, typeof(GameObject), true);
                objetivoEO.npcAEscoltar = (Npc)EditorGUILayout.ObjectField("Npc a escoltar:", objetivoEO.npcAEscoltar, typeof(Npc), true);
                break;

            case TiposObjetivos.HablarBoss:
                break;

            case TiposObjetivos.HablarNpc:
                HablarNpc objetivoHN = (HablarNpc)objetivo.downCast();

                objetivoHN.dialogoIndex = EditorGUILayout.IntField("Dialogo index:", objetivoHN.dialogoIndex);
                objetivoHN.npcAHablar = (Npc)EditorGUILayout.ObjectField("Npc a hablar:", objetivoHN.npcAHablar, typeof(Npc), true);
                break;

            case TiposObjetivos.IrAZona:
                break;

            default:
                break;
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
}
//==========================================================================================================================


