using UnityEngine;
using System.Collections;

public class PlayerInteract : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomSpeed = 2f;

    private Vector3 originalPos;
    private Quaternion originalRot;
    private InteractableItem currentItem = null;
    private bool isZoomed = false;
    private bool isMoving = false;

    void Start()
    {
        originalPos = mainCamera.transform.position;
        originalRot = mainCamera.transform.rotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                InteractableItem clickedItem = hit.collider.GetComponent<InteractableItem>();

                if (clickedItem != null)
                {
                    if (!isZoomed)
                    {
                        // KLIK 1: Zoom ke target
                        currentItem = clickedItem;
                        StartCoroutine(MoveCamera(currentItem.cameraTargetSpot.position, currentItem.cameraTargetSpot.rotation, true));
                    }
                    else if (isZoomed && clickedItem == currentItem)
                    {
                        // KLIK 2: Munculkan UI JIKA objek tersebut adalah brankas
                        BrankasSystem brankas = hit.collider.GetComponent<BrankasSystem>();
                        if (brankas != null) brankas.BukaUIPin();
                    }
                }
            }
        }
    }

    IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot, bool zoomStatus)
    {
        isMoving = true;
        float t = 0;
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        while (t < 1)
        {
            t += Time.deltaTime * zoomSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            mainCamera.transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }

        isZoomed = zoomStatus;
        isMoving = false;
    }

    public void ResetKamera()
    {
        if (isZoomed && !isMoving)
        {
            if (currentItem != null)
            {
                BrankasSystem brankas = currentItem.GetComponent<BrankasSystem>();
                if (brankas != null) brankas.TutupUIPin();
            }
            StartCoroutine(MoveCamera(originalPos, originalRot, false));
            currentItem = null;
        }
    }
}