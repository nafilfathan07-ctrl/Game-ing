using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [Header("Komponen UI")]
    public CanvasGroup logoCanvasGroup;

    [Header("Pengaturan Waktu")]
    public float durasiFade = 1.5f;   // Kecepatan fade in & fade out
    public float durasiTampil = 2f;   // Berapa lama gambar diam di layar
    public string namaSceneBerikutnya = "Stage0"; // Ganti nama scene tujuan

    private bool sedangTransisi = false;
    private bool skipDitekan = false;

    void Start()
    {
        // Mulai dari transparan total (Alpha = 0)
        logoCanvasGroup.alpha = 0f;
        
        // Jalankan urutan animasinya
        StartCoroutine(SequenceTitleScreen());
    }

    void Update()
    {
        // Deteksi kalau pemain mau skip (Spasi, Enter, atau Klik Kiri)
        if (!skipDitekan && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            skipDitekan = true;
        }
    }

    IEnumerator SequenceTitleScreen()
    {
        sedangTransisi = true;
        float timer = 0f;

        // 1. FADE IN (Muncul perlahan)
        while (timer < durasiFade)
        {
            if (skipDitekan) break; // Kalau di-skip, langsung keluar dari loop
            
            timer += Time.deltaTime;
            logoCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / durasiFade);
            yield return null;
        }
        logoCanvasGroup.alpha = 1f; // Pastikan gambar solid

        // 2. TAHAN (Tunggu sebentar)
        timer = 0f;
        while (timer < durasiTampil)
        {
            if (skipDitekan) break;
            
            timer += Time.deltaTime;
            yield return null;
        }

        // 3. FADE OUT (Menghilang perlahan)
        timer = 0f;
        // Kalau di-skip, fade outnya kita bikin lebih cepet
        float durasiFadeOutAktual = skipDitekan ? 0.5f : durasiFade; 

        while (timer < durasiFadeOutAktual)
        {
            timer += Time.deltaTime;
            logoCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / durasiFadeOutAktual);
            yield return null;
        }
        logoCanvasGroup.alpha = 0f;

        // 4. PINDAH SCENE
        SceneManager.LoadScene(namaSceneBerikutnya);
    }
}