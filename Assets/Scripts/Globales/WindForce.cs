using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForce : MonoBehaviour {
    public Material vegetationMaterial;

    public float windForce = 1;
    public float windPulse = 1;
    public float windSpeed = 1;

    float index;

	void Update () {
        Vector4 _wind = new Vector4();
        Vector4 _windSpeed = new Vector4();
        _wind = gameObject.transform.forward * windForce;

        index += Time.deltaTime;

        _wind.x = _wind.x * Mathf.Sin(windPulse * index);;
        _wind.z = _wind.z * Mathf.Sin(windPulse * index);;

        vegetationMaterial.SetVector("_Wind", _wind);

        _windSpeed.x = windSpeed;
        _windSpeed.y = windSpeed;

        vegetationMaterial.SetVector("_WindSpeed", _windSpeed);
	}
}
