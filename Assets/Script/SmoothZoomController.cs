using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothZoomController : MonoBehaviour
{
    [Header("Referensi Utama")]
    public Camera roomCamera;
    public GameObject backButton;
    public MonoBehaviour scriptMuterRuangan; 

    [Header("Pengaturan")]
    public float transitionDuration = 0.8f;

    // Menyimpan posisi lokal terhadap CameraPivot
    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;

    private bool isZoomed = false;
    private bool isMoving = false;

    void Start()
    {
        if (backButton != null) backButton.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isZoomed && !isMoving)
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = roomCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            InteractableItem item = hit.collider.GetComponentInParent<InteractableItem>();

            if (item != null && item.cameraTargetSpot != null)
            {
                // SIMPAN POSISI LOKAL (Karena kamera punya parent/pivot)
                originalLocalPos = roomCamera.transform.localPosition;
                originalLocalRot = roomCamera.transform.localRotation;

                if (scriptMuterRuangan != null) scriptMuterRuangan.enabled = false;
                
                StartCoroutine(MoveCamera(item.cameraTargetSpot.position, item.cameraTargetSpot.rotation, true, item.transform));
            }
        }
    }

    public void ZoomOut()
    {
        if (!isMoving && isZoomed)
        {
            // KONVERSI BALIK: Hitung titik dunia berdasarkan posisi Pivot saat ini
            Vector3 targetWorldPos = roomCamera.transform.parent.TransformPoint(originalLocalPos);
            Quaternion targetWorldRot = roomCamera.transform.parent.rotation * originalLocalRot;
            
            StartCoroutine(MoveCamera(targetWorldPos, targetWorldRot, false, null));
        }
    }

    IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot, bool zoomingIn, Transform lookAtTarget)
    {
        isMoving = true;
        if (backButton != null) backButton.SetActive(false);

        Vector3 startPos = roomCamera.transform.position;
        Quaternion startRot = roomCamera.transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            float smoothStep = Mathf.SmoothStep(0f, 1f, t);

            roomCamera.transform.position = Vector3.Lerp(startPos, targetPos, smoothStep);
            
            if (zoomingIn && lookAtTarget != null)
            {
                Quaternion forcedLook = Quaternion.LookRotation(lookAtTarget.position - roomCamera.transform.position);
                roomCamera.transform.rotation = Quaternion.Slerp(startRot, forcedLook, smoothStep);
            }
            else
            {
                roomCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, smoothStep);
            }

            yield return null;
        }

        roomCamera.transform.position = targetPos;
        if (!zoomingIn) roomCamera.transform.rotation = targetRot;

        isZoomed = zoomingIn;
        isMoving = false;

        if (!isZoomed && scriptMuterRuangan != null)
        {
            scriptMuterRuangan.enabled = true;
        }

        if (isZoomed && backButton != null)
        {
            backButton.SetActive(true);
        }
    }
}