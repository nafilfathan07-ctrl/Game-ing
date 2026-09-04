using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LaptopController : MonoBehaviour
{
    public GameObject laptopCanvas;
    public RectTransform laptopPanel;
    
    [Header("Efek Layar")]
    public Image blackScreen; 
    public float durasiAnimasi = 0.5f;
    public float durasiBooting = 0.4f; 

    public bool hasFlashdisk = true; 

    private bool isLaptopOpen = false;
    private bool isAnimating = false;
    private Vector2 posisiTutup = new Vector2(0, -1500f);
    private Vector2 posisiBuka = Vector2.zero;

    void Start()
    {
        laptopCanvas.SetActive(false);
        laptopPanel.anchoredPosition = posisiTutup;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && hasFlashdisk && !isAnimating)
        {
            if (isLaptopOpen)
            {
                StartCoroutine(AnimasikanLaptop(posisiBuka, posisiTutup, false));
            }
            else
            {
                StartCoroutine(AnimasikanLaptop(posisiTutup, posisiBuka, true));
            }
        }
    }

    private IEnumerator AnimasikanLaptop(Vector2 dari, Vector2 ke, bool buka)
    {
        isAnimating = true;
        
        if (buka) 
        {
            laptopCanvas.SetActive(true);
            // Matikan layar (hitamkan) setiap kali laptop baru mau dibuka
            Color c = blackScreen.color;
            c.a = 1f;
            blackScreen.color = c;
            blackScreen.gameObject.SetActive(true);
        }
        else
        {
            // Matikan layar secara instan saat mulai ditutup
            blackScreen.gameObject.SetActive(true);
        }

        float waktu = 0f;
        while (waktu < durasiAnimasi)
        {
            waktu += Time.deltaTime;
            float persentase = waktu / durasiAnimasi;
            float smoothStep = Mathf.SmoothStep(0f, 1f, persentase);
            
            laptopPanel.anchoredPosition = Vector2.Lerp(dari, ke, smoothStep);
            yield return null;
        }

        laptopPanel.anchoredPosition = ke;
        
        if (buka)
        {
            // Setelah laptop selesai naik, tunggu 0.2 detik, lalu nyalakan layar
            yield return new WaitForSeconds(0.2f);
            yield return StartCoroutine(NyalakanLayar());
        }
        else 
        {
            laptopCanvas.SetActive(false);
        }

        isLaptopOpen = buka;
        isAnimating = false;
    }

    private IEnumerator NyalakanLayar()
    {
        float waktu = 0f;
        Color c = blackScreen.color;
        
        while (waktu < durasiBooting)
        {
            waktu += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, waktu / durasiBooting);
            blackScreen.color = c;
            yield return null;
        }
        
        blackScreen.gameObject.SetActive(false);
    }
    
    public void AmbilFlashdisk()
    {
        hasFlashdisk = true;
    }
}