using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class DraggableDocument : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [Header("Settings")]
    [SerializeField] private bool bringToFrontOnDrag = true;
    [SerializeField] private float dragAlpha = 0.9f;
    [SerializeField] private float dragScale = 1.05f; // Efek diangkat sedikit

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas parentCanvas;
    private Vector3 originalScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentCanvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Opsional: Bawa ke lapisan paling atas saat disentuh/diklik
        if (bringToFrontOnDrag)
        {
            transform.SetAsLastSibling();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Beri umpan balik visual saat dokumen diangkat
        canvasGroup.alpha = dragAlpha;
        rectTransform.localScale = originalScale * dragScale;
        
        // Memastikan raycast tidak terhalang oleh dirinya sendiri
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Menggeser posisi dokumen mengikuti posisi kursor (kompatibel semua Canvas Scale)
        if (parentCanvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Kembalikan efek visual ke kondisi semula saat dilepas
        canvasGroup.alpha = 1f;
        rectTransform.localScale = originalScale;
        canvasGroup.blocksRaycasts = true;
    }
}