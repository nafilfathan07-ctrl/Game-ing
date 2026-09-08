using UnityEngine;

public class HouseClickTransition : MonoBehaviour
{
    public string namaSceneTujuan = "Stage 1";
    public SceneFader fader;

    [Header("Pengaturan Audio")]
    public AudioClip suaraKlik; // Tarik file mp3/wav efek suara klik ke sini
    private AudioSource audioSource;

    private void Start()
    {
        // Otomatis nambahin komponen pemutar suara ke rumah biar kamu gak repot
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnMouseDown()
    {
        if (!enabled) return; 

        // 1. MATIKAN SEMUA SUARA DI SCENE
        // Cari semua objek yang punya AudioSource di dalam game saat ini
        AudioSource[] semuaSuara = FindObjectsOfType<AudioSource>();
        
        foreach (AudioSource suara in semuaSuara)
        {
            // Matikan semuanya, KECUALI pemutar suara milik rumah ini sendiri
            if (suara != audioSource)
            {
                suara.Stop();
            }
        }

        // 2. PUTAR SUARA KLIK
        if (suaraKlik != null)
        {
            audioSource.PlayOneShot(suaraKlik);
        }

        // 3. MULAI PINDAH SCENE
        if (fader != null)
        {
            fader.MulaiPindahScene(namaSceneTujuan);
        }
    }
}