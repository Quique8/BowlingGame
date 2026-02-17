using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bola : MonoBehaviour
{
    public static bool impacto;
    public static int tiradas;
    private void Start()
    {
        impacto = false;
        tiradas = 3;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bolo")
        {
            impacto = true;
        }
    }
}
