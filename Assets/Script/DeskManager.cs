using UnityEngine;

public class DeskManager : MonoBehaviour
{
    public static DeskManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject winPanel; // Panel pop-up saat menang

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OnTargetDocumentFound()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        
        // Tambahkan efek suara, partisipatif, atau transisi scene di sini
    }
}