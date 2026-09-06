using UnityEngine;
using TMPro;

public class AppViewer : MonoBehaviour
{
    public GameObject windowViewer;
    public TMP_Text titleText;
    public TMP_Text contentText;

    public GameObject windowError;
    public TMP_Text errorText;

    public void BukaFileTeks(string judul, string isi)
    {
        windowViewer.SetActive(true);
        titleText.text = judul;
        contentText.text = isi;
    }

    public void BukaError(string pesan)
    {
        windowError.SetActive(true);
        errorText.text = pesan;
    }

    public void TutupSemua()
    {
        windowViewer.SetActive(false);
        windowError.SetActive(false);
    }
}