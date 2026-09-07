using UnityEngine;

public class GeserDokumen : MonoBehaviour
{
    private Vector3 titikOffset;
    private float zKamera;

    // Pas mouse mulai nge-klik kertasnya
    void OnMouseDown()
    {
        zKamera = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        titikOffset = gameObject.transform.position - DapatkanPosisiMouse();
    }

    // Pas mouse ditahan dan digeser
    void OnMouseDrag()
    {
        Vector3 posisiBaru = DapatkanPosisiMouse() + titikOffset;
        
        // Kunci posisi Y supaya kertasnya nggak terbang ke atas/bawah, cuma geser di meja
        posisiBaru.y = transform.position.y; 
        transform.position = posisiBaru;
    }

    private Vector3 DapatkanPosisiMouse()
    {
        Vector3 posisiMouse = Input.mousePosition;
        posisiMouse.z = zKamera;
        return Camera.main.ScreenToWorldPoint(posisiMouse);
    }
}