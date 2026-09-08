using UnityEngine;

public class InspeksiFlashdisk : MonoBehaviour
{
    public float kecepatanPutar = 5f;
    public GameObject tombolAmbil; // Tombol untuk menyimpan flashdisk ke inventory/menutupnya

    void OnMouseDrag()
    {
        // Fungsi memutar objek dengan drag mouse
        float putarX = Input.GetAxis("Mouse X") * kecepatanPutar;
        float putarY = Input.GetAxis("Mouse Y") * kecepatanPutar;

        transform.Rotate(Vector3.up, -putarX, Space.World);
        transform.Rotate(Vector3.right, putarY, Space.World);
    }

    // Dipanggil saat tombol "Ambil/Simpan" ditekan
    public void SimpanFlashdisk()
    {
        if (tombolAmbil != null) tombolAmbil.SetActive(false);
        gameObject.SetActive(false); // Sembunyikan flashdisk dari layar
        
        // (Opsional: Nanti bisa ditambah kode buat masukin ke inventory di sini)
    }
}