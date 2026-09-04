using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FolderManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject fileIconPrefab;
    public Transform contentArea;
    
    [Header("App References")]
    public GameObject textEditorWindow; // Panel UI untuk baca file
    public TMP_Text textEditorContent;  // Teks di dalam UI pembaca file

    [Header("Root Data")]
    public List<FileItem> rootFiles; // Ini adalah isi desktop/folder utama

    // Menyimpan posisi folder yang sedang dibuka
    private List<FileItem> currentDirectory;
    
    // Menyimpan sejarah (history) untuk tombol Back
    private Stack<List<FileItem>> directoryHistory = new Stack<List<FileItem>>();

    private void Start()
    {
        // Saat mulai, direktori saat ini adalah root (folder paling luar)
        currentDirectory = rootFiles;
        RefreshFolder();
    }

    public void RefreshFolder()
    {
        // 1. Bersihkan ikon lama
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }

        // 2. Spawn ikon baru berdasarkan isi currentDirectory
        foreach (FileItem file in currentDirectory)
        {
            GameObject newIcon = Instantiate(fileIconPrefab, contentArea);
            
            // Set nama ikon
            TMP_Text iconText = newIcon.GetComponentInChildren<TMP_Text>();
            if (iconText != null) iconText.text = file.itemName;

            // Setup deteksi klik
            IconClickHandler clickHandler = newIcon.GetComponent<IconClickHandler>();
            if (clickHandler != null)
            {
                clickHandler.Setup(file, this);
            }
        }
    }

    // Fungsi untuk membuka folder
    public void OpenFolder(FileItem folder)
    {
        // Simpan direktori saat ini ke dalam history (untuk tombol Back)
        directoryHistory.Push(currentDirectory);
        
        // Pindah ke direktori baru dan refresh layar
        currentDirectory = folder.folderContents;
        RefreshFolder();
    }

    // Fungsi untuk tombol "Back" UI (Kembali ke folder sebelumnya)
    public void GoBack()
    {
        if (directoryHistory.Count > 0)
        {
            // Ambil direktori terakhir dari history
            currentDirectory = directoryHistory.Pop();
            RefreshFolder();
        }
    }

    // Fungsi untuk membuka file teks
    public void OpenFile(FileItem file)
    {
        if (textEditorWindow != null && textEditorContent != null)
        {
            textEditorWindow.SetActive(true);
            textEditorContent.text = file.fileTextContent;
            
            // Buat jendela ini berada di tumpukan paling depan
            textEditorWindow.transform.SetAsLastSibling(); 
        }
    }
}