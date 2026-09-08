using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    [Header("Posisi Kamera")]
    public Transform cameraTargetSpot;

    [Header("Efek Suara")]
    public AudioClip suaraKlikBenda; // Tarik mp3/wav suara barang disentuh ke sini
    private AudioSource audioSource;

    private void Start()
    {
        // Bikin komponen AudioSource otomatis pas game mulai, jadi kamu nggak perlu repot
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // Biar suaranya 2D dan kedengaran jelas di mana aja
    }

    // Fungsi ini otomatis jalan kalau bendanya diklik pakai mouse
    private void OnMouseDown()
    {
        // Putar efek suara!
        if (audioSource != null && suaraKlikBenda != null)
        {
            audioSource.PlayOneShot(suaraKlikBenda);
        }

        // (Kalau nanti kamu punya script lain buat mindahin kamera ke cameraTargetSpot, 
        // panggilannya biasanya ditaruh di sini juga)
    }
}