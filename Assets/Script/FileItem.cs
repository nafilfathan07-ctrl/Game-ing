using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileItem
{
    public string itemName;
    public bool isFolder;
    
    [Header("Jika ini Folder:")]
    public List<FileItem> folderContents; 
    
    [Header("Jika ini File:")]
    [TextArea]
    public string fileTextContent; 
}