using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FileSystemManager : MonoBehaviour
{
    public Dictionary<string, string> fileData = new Dictionary<string, string>();
    public FileManagerUI fileManagerUI;

    private void Start()
    {
        TambahFile("surat_wasiat.pdf", "@@^%&!@$#*&^!@#^&*(GIBBERISH_DATA)");
        TambahFile("sys_log.txt", "[10:42:15] System boot.\n[10:45:01] Network established.\n[22:30:15] UNKNOWN CONNECTION DETECTED.");
        TambahFile("config.ini", "[NETWORK]\nIP_TARGET=192.168.1.254\nPORT=80\nSTATUS=LOCKED");
        TambahFile("hidden_dir/", "");
    }

    public void TambahFile(string namaFile, string isi = "(Empty file)")
    {
        if (!fileData.ContainsKey(namaFile))
        {
            fileData.Add(namaFile, isi);
        }
        else
        {
            fileData[namaFile] = isi; 
        }
        
        if (fileManagerUI != null) fileManagerUI.RefreshUI();
    }

    public void HapusFile(string namaFile)
    {
        if (fileData.ContainsKey(namaFile))
        {
            fileData.Remove(namaFile);
            if (fileManagerUI != null) fileManagerUI.RefreshUI();
        }
    }

    public List<string> GetDaftarFile()
    {
        return fileData.Keys.ToList();
    }
}