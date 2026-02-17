using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorSuelo : MonoBehaviour
{
    public float angulo_min, angulo_max;

    public bool HaCaido()
    {
        float angulo = Vector3.Angle(transform.up, Vector3.up);
        return angulo < angulo_min || angulo > angulo_max;
    }
}
