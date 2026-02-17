using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Juego : MonoBehaviour
{
    public GameObject canvas;
    public Slider barraFuerza, barraDireccion;
    public Rigidbody bolaRB;
    public Transform pista, camaraBola;
    public Animator sujecion, recogedor, recogeBolos;
    public Collider barra;
    public GameObject[] pines, pinesDePie;
    public Rigidbody[] rbPines;
    public TextMeshProUGUI puntuacion;
    Vector3 subidaBolo, posicionInicialBola,posicionCamaraIncio;
    Quaternion rotacionInicialBola;
    public enum BolaState {quieta ,lanzada}
    public BolaState bolaState;
    public float velocidadF, velocidadD, contador;
    public int pinesCaidos;
    public static int puntos;
    private bool movimientoBarraF, movimientoBarraD, bolaLanzada, subir, drch, recoger;

    void Start()
    {
        #region InicializacionesVariables
        contador = 10f;
        puntos = 0;
        pinesCaidos = 0;
        movimientoBarraF = true;
        movimientoBarraD = false;
        drch = true;
        subir = true;
        bolaLanzada = false;
        recoger = true;
        subidaBolo = new Vector3 (0f, 0.2f, 0f);
        posicionInicialBola = bolaRB.transform.position;
        rotacionInicialBola = bolaRB.transform.rotation;
        posicionCamaraIncio = camaraBola.transform.position;
        #endregion
        SubirRecogedor();
    }

    void Update()
    {
        Movimiento_BarraF();
        Movimiento_BarraD();
        PararBarras();
        puntuacion.text = puntos.ToString();
        if (bolaState == BolaState.lanzada)
        {
            contador -= 1 * Time.deltaTime;
            if ((Bola.impacto || contador <= 0) && recoger)
            {
                StartCoroutine(Tiro());
            }
        }
        if(pinesCaidos >= 10 || Bola.tiradas <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }
    void Movimiento_BarraF()
    {
        if (movimientoBarraF)
        {
            if (barraFuerza.value < barraFuerza.maxValue && subir)
            {
                barraFuerza.value += velocidadF * Time.deltaTime;
            }
            else
            {
                subir = false;
            }
            if (barraFuerza.value > barraFuerza.minValue && !subir)
            {
                barraFuerza.value -= velocidadF * Time.deltaTime;
            }
            else
            {
                subir = true;
            }
        }
        

    }
    void Movimiento_BarraD()
    {
        if (movimientoBarraD)
        {
            if (barraDireccion.value < barraDireccion.maxValue && drch)
            {
                barraDireccion.value += velocidadD * Time.deltaTime;
            }
            else
            {
                drch = false;
            }
            if (barraDireccion.value > barraDireccion.minValue && !drch)
            {
                barraDireccion.value -= velocidadD * Time.deltaTime;
            }
            else
            {
                drch = true;
            }
        }
    }

    void PararBarras()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (movimientoBarraF)
            {
                movimientoBarraF = false;
                movimientoBarraD = true;
            }
            else if (movimientoBarraD && !bolaLanzada)
            {
                movimientoBarraD = false;
                bolaLanzada = true;
                MoverBola(barraFuerza.value, barraDireccion.value);
                canvas.SetActive(false);
                contador = 10f;
            }
        }
    }

    void MoverBola(float fuerza, float direccion)
    {
        float angulo = (direccion) * 90;
        float radianes = Mathf.Deg2Rad * angulo;
        Quaternion rotacion = Quaternion.Euler(0, radianes, 0);
        Vector3 direccionMovimiento = rotacion * pista.forward;
        bolaRB.AddForce(direccionMovimiento * fuerza, ForceMode.Impulse);
        bolaState = BolaState.lanzada;
    }

    void SubirRecogedor()
    {
        sujecion.SetBool("Bajar", false);
        recogedor.SetBool("Bajar", false);
        sujecion.SetBool("Subir", true);
        recogedor.SetBool("Subir", true);
    }
    void BajarRecogedor()
    {
        sujecion.SetBool("Subir", false);
        recogedor.SetBool("Subir", false);
        sujecion.SetBool("Bajar", true);
        recogedor.SetBool("Bajar", true);
    }

    void Barrer()
    {
        recogeBolos.SetBool("Volver", false);
        recogeBolos.SetBool("Barrer", true);
        barra.enabled = true;
    }
    void Volver()
    {
        recogeBolos.SetBool("Barrer", false);
        recogeBolos.SetBool("Volver", true);
        barra.enabled = false;
    }
    private System.Collections.IEnumerator AnimSubirB(Vector3 start, Vector3 end, int i)
    {
        float duration = 1f;
        float elapsed = 0f;
        rbPines[i].useGravity = false;
        while (elapsed < duration)
        {
            pines[i].transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    private System.Collections.IEnumerator AnimBajarB(Vector3 start, Vector3 end, int i)
    {
        float duration = 1f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            pines[i].transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rbPines[i].velocity = Vector3.zero;
        rbPines[i].angularVelocity = Vector3.zero;
        rbPines[i].useGravity = true;
    }

    IEnumerator Tiro()
    {
        recoger = false;
        yield return new WaitForSecondsRealtime(2f);
        BajarRecogedor();
        bolaRB.transform.SetPositionAndRotation(posicionInicialBola, rotacionInicialBola);
        bolaRB.velocity = Vector3.zero;
        bolaRB.angularVelocity = Vector3.zero;
        canvas.SetActive(true);
        Camara seguimiento = camaraBola.GetComponent<Camara>();
        if (seguimiento != null)
        {
            seguimiento.VolverSuavemente(posicionCamaraIncio);
        }
        Bola.tiradas--;
        yield return new WaitForSecondsRealtime(4f);
        for (int i = 0; i < pines.Length; i++)
        {
            DetectorSuelo detector = pines[i].GetComponent<DetectorSuelo>();
            if (detector != null && !detector.HaCaido())
            {
                StartCoroutine(AnimSubirB(pines[i].transform.position, pines[i].transform.position + subidaBolo, i));
            }
            else
            {
                pinesCaidos++;
                if(pinesCaidos > puntos)
                {
                    puntos += pinesCaidos - puntos;
                }
            }
        }
        yield return new WaitForSecondsRealtime(1f);
        Barrer();
        yield return new WaitForSecondsRealtime(3f);
        Volver();
        yield return new WaitForSecondsRealtime(3f);
        SubirRecogedor();
        for (int i = 0; i < pines.Length; i++)
        {
            DetectorSuelo detector = pines[i].GetComponent<DetectorSuelo>();
            if (detector != null && !detector.HaCaido())
            {
                StartCoroutine(AnimBajarB(pines[i].transform.position, pines[i].transform.position - subidaBolo, i));
            }
        }
        yield return new WaitForSecondsRealtime(1.5f);
        movimientoBarraF = true;
        bolaLanzada = false;
        bolaState = BolaState.quieta;
        recoger = true;
        Bola.impacto = false;
        pinesCaidos = 0;
    }
}


