using UnityEngine;
using System.Collections;

public class InspeksiFoto : MonoBehaviour
{
    [Header("Pengaturan Posisi Inspeksi")]
    [Tooltip("0 = Di meja, 1 = Nempel Kamera. Angka 0.3 berarti naik dikit ke arah kamera.")]
    [Range(0f, 1f)]
    public float persentaseJarak = 0.3f; 
    
    [Tooltip("Kecepatan gerak foto bolak-balik")]
    public float kecepatanGerak = 3f; 
    public float kecepatanPutar = 5f;
    public GameObject tombolBackFoto;

    private Vector3 posisiAwal;
    private Quaternion rotasiAwal;
    private bool sedangDiinspeksi = false;
    private bool sedangBergerak = false;

    void Start()
    {
        // Simpan posisi awal di tumpukan meja
        posisiAwal = transform.position;
        rotasiAwal = transform.rotation;
        
        if (tombolBackFoto != null) tombolBackFoto.SetActive(false);
    }

    void OnMouseDown()
    {
        // Kalau diklik dan belum diinspeksi
        if (!sedangDiinspeksi && !sedangBergerak)
        {
            Transform kamera = Camera.main.transform;
            
            // MAGIC-NYA DI SINI:
            // Cari titik di antara meja (posisiAwal) dan Kamera. 
            // Kalau persentasenya 0.3, berarti fotonya cuma ditarik 30% jalan menuju kamera.
            Vector3 posisiTarget = Vector3.Lerp(posisiAwal, kamera.position, persentaseJarak);
            
            // Rotasi tetap menghadap sejajar ke layar kamera biar enak dibaca
            Quaternion rotasiMenghadapKamera = kamera.rotation;

            StartCoroutine(PindahPosisiMulus(posisiTarget, rotasiMenghadapKamera, true));
        }
    }

    void OnMouseDrag()
    {
        // Fitur putar-putar
        if (sedangDiinspeksi && !sedangBergerak)
        {
            float putarX = Input.GetAxis("Mouse X") * kecepatanPutar;
            float putarY = Input.GetAxis("Mouse Y") * kecepatanPutar;

            transform.Rotate(Vector3.up, -putarX, Space.World);
            transform.Rotate(Vector3.right, putarY, Space.World);
        }
    }

    public void KembalikanFoto()
    {
        // Balik ke meja
        if (sedangDiinspeksi && !sedangBergerak)
        {
            StartCoroutine(PindahPosisiMulus(posisiAwal, rotasiAwal, false));
        }
    }

    IEnumerator PindahPosisiMulus(Vector3 targetPos, Quaternion targetRot, bool statusInspeksi)
    {
        sedangBergerak = true;
        float t = 0;
        
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (t < 1)
        {
            t += Time.deltaTime * kecepatanGerak;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }

        sedangDiinspeksi = statusInspeksi;
        sedangBergerak = false;

        if (tombolBackFoto != null)
        {
            tombolBackFoto.SetActive(sedangDiinspeksi);
        }
    }
}