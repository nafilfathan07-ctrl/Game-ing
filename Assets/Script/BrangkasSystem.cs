using UnityEngine;
using TMPro;
using System.Collections;

public class BrankasSystem : MonoBehaviour
{
    public GameObject uiBrankasPanel; 
    public TMP_Text teksDisplay;      
    public string pinBenar = "2072026";
    private string inputSaatIni = "";

    public void TekanAngka(string angka)
    {
        if (inputSaatIni.Length < 7) 
        {
            inputSaatIni += angka;
            UpdateDisplay();
        }
    }

    public void HapusInput()
    {
        inputSaatIni = "";
        UpdateDisplay();
    }

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
        if (uiBrankasPanel != null) 
        {
            uiBrankasPanel.SetActive(false);
        }

        // --- RESET OTOMATIS SAAT UI TERTUTUP KARENA SUKSES ---
        KlikBrankasUI klikUI = GetComponent<KlikBrankasUI>();
        if (klikUI != null)
        {
            klikUI.ResetHitunganKlik();
        }
    }

    public void BukaUIPin() 
    { 
        if (uiBrankasPanel != null) uiBrankasPanel.SetActive(true); 
    }

    // --- FUNGSI UNTUK TOMBOL BACK ---
    public void TutupUIPin() 
    { 
        inputSaatIni = ""; 
        UpdateDisplay(); 
        
        if (uiBrankasPanel != null) 
        {
            uiBrankasPanel.SetActive(false); 
        }

        // --- RESET OTOMATIS SAAT TOMBOL BACK DITEKAN ---
        KlikBrankasUI klikUI = GetComponent<KlikBrankasUI>();
        if (klikUI != null)
        {
            klikUI.ResetHitunganKlik();
        }
    }
}