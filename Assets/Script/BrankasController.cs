using UnityEngine;
using TMPro; 
using System.Collections;

public class BrankasController : MonoBehaviour
{
    public TMP_Text teksDisplay;
    public string pinBenar = "2072026"; // PIN barumu
    private string inputSaatIni = "";

    // Dipanggil saat tombol 0-9 ditekan
    public void TekanAngka(string angka)
    {
        if (inputSaatIni.Length < 7) // Batas sudah dinaikkan jadi 7 digit
        {
            inputSaatIni += angka;
            UpdateDisplay();
        }
    }

    // Dipanggil saat tombol 'C' ditekan
    public void HapusInput()
    {
        inputSaatIni = "";
        UpdateDisplay();
    }

    // Dipanggil saat tombol 'OK' ditekan
    public void KonfirmasiPIN()
    {
        if (inputSaatIni == pinBenar)
        {
            teksDisplay.text = "SUKSES";
            teksDisplay.color = Color.green;
            Debug.Log("Brankas Terbuka! Dapat Flashdisk.");
            
            StartCoroutine(TutupUIAfterDelay());
        }
        else
        {
            teksDisplay.text = "ERROR";
            teksDisplay.color = Color.red;
            inputSaatIni = ""; // Reset
            Invoke("KembalikanDisplay", 1f); // Balik ke normal setelah 1 detik
        }
    }

    private void UpdateDisplay()
    {
        teksDisplay.text = inputSaatIni == "" ? "----" : inputSaatIni;
    }

    private void KembalikanDisplay()
    {
        teksDisplay.color = Color.white; 
        UpdateDisplay();
    }

    IEnumerator TutupUIAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false); // Menyembunyikan layar brankas
    }
}