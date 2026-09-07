using UnityEngine;

public class BukaPinBrankas : MonoBehaviour
{
    [Header("Panel UI Numpad Brankas")]
    public GameObject uiNumpadBrankas; // Tarik UI Numpad lu ke sini di Inspector

    // Fungsi ini dipanggil pas klik kedua setelah kamera selesai zoom
    public void TampilkanPin()
    {
        if (uiNumpadBrankas != null)
        {
            uiNumpadBrankas.SetActive(true); // Munculin UI PIN
        }
    }

    // Fungsi ini ditaruh di tombol "Back" buat nutup UI PIN-nya lagi
    public void TutupPin()
    {
        if (uiNumpadBrankas != null)
        {
            uiNumpadBrankas.SetActive(false); // Sembunyiin UI PIN
        }
    }
}