using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform bola; 
    public float smoothTime;
    public float maxZSeguir;
    private bool volviendo = false;
    private Vector3 destinoVuelta;

    private Vector3 velocidad = Vector3.zero; 
    private Vector3 desplazamiento; 
    private Vector3 posicionFinal;
    private bool seguir = true;

    void Start()
    {
        if (bola != null)
        {
            desplazamiento = transform.position - bola.position;
        }
    }

    void LateUpdate()
    {
        if (bola == null) return;

        if (seguir)
        {
            Vector3 targetPosition = bola.position + desplazamiento;

            if (bola.position.z > maxZSeguir)
            {
                seguir = false;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocidad, smoothTime);
            }
        }
        else if (volviendo)
        {
            transform.position = Vector3.SmoothDamp(transform.position, destinoVuelta, ref velocidad, smoothTime);

            if (Vector3.Distance(transform.position, destinoVuelta) < 0.0005f)
            {
                transform.position = destinoVuelta;
                volviendo = false;
                seguir = true;
                velocidad = Vector3.zero;

                // Recalcular desplazamiento para el nuevo seguimiento
                desplazamiento = transform.position - bola.position;
            }
        }
    }
    public void VolverSuavemente(Vector3 destino)
    {
        volviendo = true;
        destinoVuelta = destino;
        velocidad = Vector3.zero;
    }
}
