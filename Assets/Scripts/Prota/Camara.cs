using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
//==============================================================================================================
public class Camara : MonoBehaviour {
    public Transform target;
    public float suavizado;
    public float altura;
    public float distancia;
    public float angularVelocity = 10f;
    public float mouseOffsetScale = 0.001f;
    public float fadeInVelocity = 1f;

    Vector3 _posicionFinal;

    Vector3 _pivotPoint;
    float _angle = 180;

    private List<RaycastHit> _previousIntersectingMeshes;
    private List<RaycastHit> _currentIntersectingMeshes;
    private Dictionary<string, Material> _sharedMaterials;

    private const float TARGET_ALPHA = 0.3f;

    public LayerMask layerToFade;

    void Start() {
        _pivotPoint = new Vector3();
        _pivotPoint = transform.position;

        _previousIntersectingMeshes = new List<RaycastHit>();
        _currentIntersectingMeshes = new List<RaycastHit>();
        _sharedMaterials = new Dictionary<string, Material>();
    }
    //----------------------------------------------------------------------------------------------------------
	void Update () {
        if (!target)
            return;

        transform.LookAt(target.position + cameraMouseOffset());

        Panning();
        
        _pivotPoint = new Vector3(target.position.x + Mathf.Sin(_angle * Mathf.Deg2Rad) * distancia,
                                  altura + target.position.y,
                                  target.position.z + Mathf.Cos(_angle * Mathf.Deg2Rad) * distancia);
        _pivotPoint += cameraMouseOffset();

        if((transform.position - _pivotPoint).magnitude > 0.05f)
            transform.position = Vector3.Lerp(transform.position, _pivotPoint, Time.deltaTime * suavizado);

        clampAngle();

        for (int i = 0; i < _currentIntersectingMeshes.Count; i++){
            RaycastHit mesh = _currentIntersectingMeshes[i];
            Renderer renderer = mesh.transform.GetComponent<Renderer>();

            if (renderer){
                if (_previousIntersectingMeshes.FindIndex(m => m.transform.name == mesh.transform.name) < 0)
                    startFade(ref mesh, ref renderer);
            }
        }

        for (int i = 0; i < _previousIntersectingMeshes.Count; i++){
            RaycastHit mesh = _previousIntersectingMeshes[i];
            Renderer renderer = mesh.transform.GetComponent<Renderer>();

            if (renderer){
                if (_currentIntersectingMeshes.FindIndex(m => m.transform.name == mesh.transform.name) < 0)
                    fadeOut(ref mesh, ref renderer);
            }
        }

        _previousIntersectingMeshes = _currentIntersectingMeshes;
	}
    //----------------------------------------------------------------------------------------------------------
    void FixedUpdate() {
        if (Time.fixedTime % 0.5f != 0)
            return;
        
        _currentIntersectingMeshes = Physics.RaycastAll(transform.position, 
                                                        transform.forward, 
                                                        (target.transform.position - transform.position).magnitude - 1f, 
                                                        layerToFade).ToList();
    }
    //----------------------------------------------------------------------------------------------------------
    void startFade(ref RaycastHit mesh, ref Renderer renderer) {
        if (!_sharedMaterials.ContainsKey(renderer.sharedMaterial.name))
            _sharedMaterials.Add(renderer.sharedMaterial.name, renderer.sharedMaterial);

        renderer.material.SetFloat("_Mode", 2);
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        renderer.material.SetInt("_ZWrite", 0);
        renderer.material.DisableKeyword("_ALPHATEST_ON");
        renderer.material.EnableKeyword("_ALPHABLEND_ON");
        renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        renderer.material.renderQueue = 3000;

        iTween.FadeTo(mesh.transform.gameObject,
                        iTween.Hash("alpha", 1f,
                                    "amount", TARGET_ALPHA / _currentIntersectingMeshes.Count, 
                                    "time", fadeInVelocity));
    }
    //----------------------------------------------------------------------------------------------------------
    void fadeOut(ref RaycastHit mesh, ref Renderer renderer){
        iTween.FadeTo(mesh.transform.gameObject,
                      iTween.Hash("alpha", renderer.material.color.a,
                                  "amount", 1f,
                                  "time", fadeInVelocity,
                                  "onComplete", "endFade",
                                  "onCompleteTarget", gameObject,
                                  "onCompleteParams", renderer));
    }
    //----------------------------------------------------------------------------------------------------------
    void endFade(object objRenderer){
        Renderer renderer = (Renderer)objRenderer;
        renderer.material.SetFloat("_Mode", 0);
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        renderer.material.SetInt("_ZWrite", 1);
        renderer.material.DisableKeyword("_ALPHATEST_ON");
        renderer.material.DisableKeyword("_ALPHABLEND_ON");
        renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        renderer.material.renderQueue = -1;

        string key = renderer.material.name.Replace("(Instance)", string.Empty);
        key = key.Replace(" ", string.Empty);
        Destroy(renderer.material);
        renderer.sharedMaterial = _sharedMaterials[key];
    }
    //----------------------------------------------------------------------------------------------------------
    void Panning(){
        if (Input.GetKey(KeyCode.E))
            _angle -= Time.deltaTime * angularVelocity;

        if (Input.GetKey(KeyCode.Q))
            _angle += Time.deltaTime * angularVelocity;
    }
    //----------------------------------------------------------------------------------------------------------
    void clampAngle() {
        if (_angle > 360)
            _angle = 0;
        if (_angle < 0)
            _angle = 360;
    }
    //----------------------------------------------------------------------------------------------------------
    Vector3 cameraMouseOffset() {
        Vector3 screenCenter = new Vector3(Screen.width / 2, 0, Screen.height / 2);

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mousePosition.y;

        Vector3 mouseToCenter = mousePosition - screenCenter;
        mouseToCenter *= mouseOffsetScale;
        mouseToCenter = transform.worldToLocalMatrix.inverse * mouseToCenter;
        mouseToCenter.y = 0;

        return mouseToCenter;
    }
}
//==============================================================================================================
