using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonHome : MonoBehaviour
{
    private RectTransform rectTransform;
    public static Vector3 originalScale;
    public float scaleFactor;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }
    public void Click()
    {
        SceneManager.LoadScene(0);
    }

    public void Entra()
    {
        rectTransform.localScale = originalScale * scaleFactor;
    }

    public void Sale()
    {
        rectTransform.localScale = originalScale;
    }
}
