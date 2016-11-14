using UnityEngine;
using System.Collections;

public class Formations : MonoBehaviour
{

    protected estados _estado;
    protected int ordenPos;

    protected enum estados
    {
        normal,
        bajoOrdenes
    }

    // Use this for initialization
    void Start()
    {
        _estado = estados.normal;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void begin(int pos)
    {
        if (pos < 6)
        {
            ordenPos = pos;
            _estado = estados.bajoOrdenes;
        }
    }

    public void reset()
    {
        _estado = estados.normal;
    }
}