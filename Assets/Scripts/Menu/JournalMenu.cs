using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JournalMenu : MonoBehaviour {

    public void displayObjectives() {
        GameObject botonSeleccionado = EventSystem.current.currentSelectedGameObject;
        ObjetivoGUI objetivoBtn = botonSeleccionado.GetComponent<ObjetivoGUI>();

        if (!objetivoBtn.getMostrando()){
            for (int i = 0; i < botonSeleccionado.transform.childCount; i++){
                if (botonSeleccionado.transform.GetChild(i).name.Contains("objetivo"))
                    botonSeleccionado.transform.GetChild(i).gameObject.SetActive(true);
            }

            objetivoBtn.setMostrando(true);
        }

        else {
            for (int i = 0; i < botonSeleccionado.transform.childCount; i++){
                if (botonSeleccionado.transform.GetChild(i).name.Contains("objetivo"))
                    botonSeleccionado.transform.GetChild(i).gameObject.SetActive(false);
            }

            objetivoBtn.setMostrando(false);
        }
    }
}
