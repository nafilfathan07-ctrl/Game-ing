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

    [Header("Sistem Tembok Dinamis")]
    public Transform[] walls;
    public float angkatJarak = 8f;
    public float angkatSpeed = 6f;

    private Camera cam;
    private int currentSideIndex = 0;
    private Vector3[] posisiAwalTembok;

    void Start()
    {
        cam = GetComponent<Camera>();
        transform.LookAt(cameraPivot);

        if (walls != null && walls.Length > 0)
        {
            posisiAwalTembok = new Vector3[walls.Length];
            for (int i = 0; i < walls.Length; i++)
            {
                if (walls[i] != null)
                {
                    posisiAwalTembok[i] = walls[i].position;
                }
            }
        }
    }

    void Update()
    {
        HandlePan();
        HandleZoom();
        HandleRotationInput();
        UpdatePosisiTembok();
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
            if (walls != null && walls.Length > 0)
            {
                currentSideIndex = (currentSideIndex + 1) % walls.Length;
            }
            StartCoroutine(RotatePivot(90f));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (walls != null && walls.Length > 0)
            {
                currentSideIndex = (currentSideIndex - 1 + walls.Length) % walls.Length;
            }
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

    void UpdatePosisiTembok()
    {
        if (walls == null || walls.Length == 0) return;

        int nextSideIndex = (currentSideIndex + 1) % walls.Length;

        for (int i = 0; i < walls.Length; i++)
        {
            if (walls[i] == null) continue;

            Vector3 targetPos = posisiAwalTembok[i];

            if (i == currentSideIndex || i == nextSideIndex)
            {
                targetPos += Vector3.up * angkatJarak;
            }

            walls[i].position = Vector3.Lerp(walls[i].position, targetPos, Time.deltaTime * angkatSpeed);
        }
    }
}