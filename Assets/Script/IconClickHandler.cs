using UnityEngine;
using UnityEngine.UI;

public class IconClickHandler : MonoBehaviour
{
    private FileItem myData;
    private FolderManager manager;

    // Fungsi ini akan dipanggil oleh FolderManager saat ikon di-spawn
    public void Setup(FileItem data, FolderManager mgr)
    {
        myData = data;
        manager = mgr;

        // Otomatis menyambungkan klik tombol ke fungsi OnClickIcon
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners(); // Mencegah double klik error
            btn.onClick.AddListener(OnClickIcon);
        }
    }

    private void OnClickIcon()
    {
        if (myData.isFolder)
        {
            manager.OpenFolder(myData); // Buka folder
        }
        else
        {
            manager.OpenFile(myData);   // Buka file
        }
    }
}