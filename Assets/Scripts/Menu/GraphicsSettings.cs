using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour {
    public Dropdown calidades;
    void Start() {
        for (int i = 0; i < QualitySettings.names.Length; i++)
        Debug.Log(QualitySettings.names[i]);
    }
    public void setCalidad() {
        QualitySettings.SetQualityLevel(calidades.value);
    }
}
