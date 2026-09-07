using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenFade : MonoBehaviour
{
    [Header("Komponen UI")]
    public CanvasGroup logoCanvasGroup;

    [Header("Pengaturan Waktu")]
    public float durasiFade = 1.5f;   // Kecepatan fade in & fade out
    public float durasiTampil = 2f;   // Berapa lama gambar diam di layar
    public string namaSceneBerikutnya = "Stage0"; // Ganti nama scene tujuan

    void Start()
    {
        // Mulai dari transparan total (Alpha = 0)
        logoCanvasGroup.alpha = 0f;
        
        // Jalankan animasi transisinya
        StartCoroutine(SequenceTitleScreen());
    }

    IEnumerator SequenceTitleScreen()
    {
        float timer = 0f;

        // 1. FADE IN (Muncul perlahan)
        while (timer < durasiFade)
        {
            timer += Time.deltaTime;
            logoCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / durasiFade);
            yield return null;
        }
        logoCanvasGroup.alpha = 1f; // Pastikan gambarnya solid

        // 2. TAHAN (Biar pemain bisa menikmati logonya)
        yield return new WaitForSeconds(durasiTampil);

        // 3. FADE OUT (Menghilang perlahan)
        timer = 0f;
        while (timer < durasiFade)
        {
            timer += Time.deltaTime;
            logoCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / durasiFade);
            yield return null;
        }
        logoCanvasGroup.alpha = 0f;

        // 4. PINDAH SCENE
        SceneManager.LoadScene(namaSceneBerikutnya);
    }
}