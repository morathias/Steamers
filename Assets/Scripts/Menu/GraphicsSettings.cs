using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour {
    public Dropdown calidades;
    void Start() {
    }
    public void setCalidad() {
        QualitySettings.SetQualityLevel(calidades.value);
    }
}
