using UnityEngine;
using UnityEngine.EventSystems;

public class TargetDocument : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Klik hanya akan terdeteksi jika dokumen ini TIDAK tertutup dokumen lain di atasnya
        Debug.Log("Dokumen Target Berhasil Ditemukan!");

        if (DeskManager.Instance != null)
        {
            DeskManager.Instance.OnTargetDocumentFound();
        }
    }
}