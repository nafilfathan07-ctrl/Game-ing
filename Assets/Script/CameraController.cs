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
    public float rotationDuration = 0.3f;
    private bool isRotating = false;

    [Header("Referensi Rotasi & Posisi")]
    public Transform cameraPivot;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
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
        float moveX = -Input.GetAxis("Horizontal") * panSpeed * Time.deltaTime;
        float moveZ = -Input.GetAxis("Vertical") * panSpeed * Time.deltaTime;
        cameraPivot.Translate(moveX, 0, moveZ, Space.Self);
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            if (cam.orthographic)
            {
                cam.orthographicSize -= scroll * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            }
            else
            {
                cam.fieldOfView -= scroll * (zoomSpeed * 5f);
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom * 5f, maxZoom * 5f);
            }
        }
    }

    void HandleRotationInput()
    {
        if (isRotating) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(RotatePivot(90f));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
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
            t = t * t * (3f - 2f * t);
            cameraPivot.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        cameraPivot.rotation = endRot;
        isRotating = false;
    }
}