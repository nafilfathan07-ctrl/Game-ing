using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Pengaturan Kamera")]
    public float panSpeed = 15f;
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    [Header("Pengaturan Rotasi Tiny Room")]
    public float rotationDuration = 0.3f; // lama transisi rotasi (detik)
    private bool isRotating = false;

    [Header("Referensi Rotasi & Posisi")]
    public Transform cameraPivot;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        // Pastikan kamera selalu menghadap tepat ke pivot,
        // jadi tidak perlu tebak-tebak sudut rotasi manual.
        transform.LookAt(cameraPivot);
    }

    void Update()
    {
        HandlePan();
        HandleZoom();
        HandleRotationInput();
    }

    void HandlePan()
    {
        float moveX = Input.GetAxis("Horizontal") * panSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * panSpeed * Time.deltaTime;
        cameraPivot.Translate(moveX, 0, moveZ, Space.Self);
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    void HandleRotationInput()
    {
        // DEBUG SEMENTARA — hapus baris Debug.Log ini setelah masalah ketemu
        if (Input.anyKeyDown)
            Debug.Log("Ada tombol ditekan. isRotating = " + isRotating);

        // Cegah spam input Q/E saat animasi rotasi masih berjalan
        if (isRotating) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q terdeteksi, mulai rotasi");
            StartCoroutine(RotatePivot(90f));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E terdeteksi, mulai rotasi");
            StartCoroutine(RotatePivot(-90f));
        }
    }

    IEnumerator RotatePivot(float angle)
    {
        isRotating = true;

        Quaternion startRot = cameraPivot.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, angle, 0);

        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);
            // Smoothstep biar transisi terasa natural (mulai & berhenti halus)
            t = t * t * (3f - 2f * t);
            cameraPivot.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        // Snap presisi di akhir supaya tidak ada floating point drift
        cameraPivot.rotation = endRot;
        isRotating = false;
    }
}