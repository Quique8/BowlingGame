using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Puntos : MonoBehaviour
{
    public TextMeshProUGUI puntuacion;
    void Start()
    {
        puntuacion.text = Juego.puntos.ToString();
    }
}
