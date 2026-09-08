using UnityEngine;
using TMPro;
using System.Collections;

public class BrankasSystem : MonoBehaviour
{
    [Header("UI & PIN")]
    public GameObject uiBrankasPanel; 
    public TMP_Text teksDisplay;      
    public string pinBenar = "2072026";
    private string inputSaatIni = "";

    [Header("Reward Flashdisk")]
    public GameObject objekFlashdisk; 
    public GameObject tombolAmbil;    

    public void TekanAngka(string angka)
    {
        if (inputSaatIni.Length < 7) 
        {
            inputSaatIni += angka;
            UpdateDisplay();
        }
    }

    public void HapusInput() { inputSaatIni = ""; UpdateDisplay(); }

    public void KonfirmasiPIN()
    {
        if (inputSaatIni == pinBenar)
        {
            teksDisplay.text = "SUKSES";
            teksDisplay.color = Color.green;
            StartCoroutine(TutupUIAfterDelay()); 
        }
        else
        {
            teksDisplay.text = "ERROR";
            teksDisplay.color = Color.red;
            inputSaatIni = ""; 
            Invoke("KembalikanDisplay", 1f); 
        }
    }

    private void UpdateDisplay() { teksDisplay.text = inputSaatIni == "" ? "----" : inputSaatIni; }
    private void KembalikanDisplay() { teksDisplay.color = Color.white; UpdateDisplay(); }

    IEnumerator TutupUIAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        if (uiBrankasPanel != null) uiBrankasPanel.SetActive(false);

        // --- SISTEM OTOMATIS MUNCULIN FLASHDISK DI TENGAH ---
        if (objekFlashdisk != null) 
        {
            objekFlashdisk.SetActive(true);
            
            Transform kamera = Camera.main.transform;
            
            // Hitung titik persis di tengah (50% / 0.5f) antara Brankas dan Kamera
            Vector3 titikTengah = Vector3.Lerp(transform.position, kamera.position, 0.5f);
            
            // Pindahkan flashdisk ke titik tersebut
            objekFlashdisk.transform.position = titikTengah;
            
            // Buat rotasinya menghadap sejajar dengan kamera biar rapi
            objekFlashdisk.transform.rotation = kamera.rotation;
        }

        if (tombolAmbil != null) tombolAmbil.SetActive(true);

        KlikBrankasUI klikUI = GetComponent<KlikBrankasUI>();
        if (klikUI != null) klikUI.ResetHitunganKlik();
    }

    public void BukaUIPin() { if (uiBrankasPanel != null) uiBrankasPanel.SetActive(true); }

    public void TutupUIPin() 
    { 
        inputSaatIni = ""; UpdateDisplay(); 
        if (uiBrankasPanel != null) uiBrankasPanel.SetActive(false); 

        KlikBrankasUI klikUI = GetComponent<KlikBrankasUI>();
        if (klikUI != null) klikUI.ResetHitunganKlik();
    }
}