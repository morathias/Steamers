using UnityEngine;
using System.Collections;

public class AnimacionArbol : MonoBehaviour {
    Vector4 _viento;
    public float _frecuencia = 0.75f;
    //--------------------------------------------------------------------------------------
	void Start () {
        _viento = new Vector4(0.85f, 0.075f, 0.4f, 0.5f);
        Shader.SetGlobalColor("_Wind", _viento);
	}
	//--------------------------------------------------------------------------------------
	void Update () {
	    Color vientoRGBA = _viento * (Mathf.Sin(Time.realtimeSinceStartup * _frecuencia));
        vientoRGBA.a = _viento.w;
        Shader.SetGlobalColor("_Wind", vientoRGBA);
	}
    //--------------------------------------------------------------------------------------
}
