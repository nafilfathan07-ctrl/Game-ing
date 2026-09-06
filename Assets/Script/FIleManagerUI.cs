using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FileManagerUI : MonoBehaviour
{
    public FileSystemManager fileSystemManager;
    public AppViewer appViewer; 
    public GameObject fileIconPrefab;
    public Transform contentArea;

    private void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }

        if (fileSystemManager == null) return;

        foreach (KeyValuePair<string, string> file in fileSystemManager.fileData)
        {
            GameObject icon = Instantiate(fileIconPrefab, contentArea);
            FileIcon scriptIkon = icon.GetComponent<FileIcon>();
            if (scriptIkon != null)
            {
                scriptIkon.Setup(file.Key, file.Value, appViewer);
            }
        }
    }
}