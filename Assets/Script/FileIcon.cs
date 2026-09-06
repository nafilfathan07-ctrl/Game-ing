using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FileIcon : MonoBehaviour
{
    public TMP_Text namaFileText;
    
    private string namaFile;
    private string isiFile;
    private AppViewer appViewer;

    public void Setup(string nama, string isi, AppViewer viewer)
    {
        namaFile = nama;
        isiFile = isi;
        appViewer = viewer;
        namaFileText.text = nama;
        
        Button btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(ProsesKlik);
    }

    private void ProsesKlik()
    {
        if (namaFile.EndsWith(".pdf"))
        {
            appViewer.BukaError("FATAL ERROR: " + namaFile + "\n\nENCRYPTED PAYLOAD DETECTED.\nCANNOT OPEN VIA GRAPHICAL INTERFACE.");
        }
        else
        {
            appViewer.BukaFileTeks(namaFile, isiFile);
        }
    }
}