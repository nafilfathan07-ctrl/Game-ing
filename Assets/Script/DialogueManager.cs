using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public enum Speaker { Left, Right, None }

// Class untuk data tiap baris cerita
[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 4)]
    public string text;
    public Speaker activeSpeaker = Speaker.None;
    public Sprite leftPortraitOverride;  // Opsional: untuk ganti ekspresi wajah
    public Sprite rightPortraitOverride;
}

public class DialogueManager : MonoBehaviour
{
    [Header("Referensi Portrait")]
    public Image leftPortrait;
    public Image rightPortrait;

    [Header("Referensi Teks")]
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    [Header("Pengaturan Efek")]
    public float typingSpeed = 0.03f;      // Detik per karakter
    public float colorTransitionSpeed = 6f; // Kecepatan transisi cahaya
    [Range(0f, 1f)] public float inactiveBrightness = 0.4f; // Rentang diperlebar sampai 0 (Hitam)

    [Header("Data Cerita")]
    public DialogueLine[] lines;

    private Material leftMat;
    private Material rightMat;
    private int currentIndex = 0;
    private bool isTyping = false;
    private Coroutine typingRoutine;
    private Coroutine leftFadeRoutine;
    private Coroutine rightFadeRoutine;

    void Start()
    {
        // Bikin instance material sendiri-sendiri biar tidak saling menimpa
        leftMat = new Material(leftPortrait.material);
        rightMat = new Material(rightPortrait.material);
        leftPortrait.material = leftMat;
        rightPortrait.material = rightMat;

        ShowLine(0);
    }

    void Update()
    {
        // Klik mouse kiri atau tekan Space buat lanjut dialog
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Kalau teks masih mengetik, langsung munculkan semua kalimatnya
                CompleteTyping();
            }
            else
            {
                // Kalau teks sudah selesai, lanjut ke baris berikutnya
                NextLine();
            }
        }
    }

    void ShowLine(int index)
    {
        currentIndex = index;
        DialogueLine line = lines[index];

        nameText.text = line.speakerName;

        // Ganti sprite jika ada gambar ekspresi baru yang dimasukkan
        if (line.leftPortraitOverride != null) leftPortrait.sprite = line.leftPortraitOverride;
        if (line.rightPortraitOverride != null) rightPortrait.sprite = line.rightPortraitOverride;

        SetActiveSpeaker(line.activeSpeaker);

        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = StartCoroutine(TypeText(line.text));
    }

    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void CompleteTyping()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        dialogueText.text = lines[currentIndex].text;
        isTyping = false;
    }

    void NextLine()
    {
        int next = currentIndex + 1;
        if (next < lines.Length)
        {
            ShowLine(next);
        }
        else
        {
            // Kalau sudah habis, bisa ditambahkan aksi lain, misal menutup dialog
            Debug.Log("Dialog selesai.");
        }
    }

    void SetActiveSpeaker(Speaker speaker)
    {
        // Target = 1 (Terang) jika aktif, Target = 0 (Gelap) jika diam
        float leftTarget = (speaker == Speaker.Left) ? 1f : 0f;
        float rightTarget = (speaker == Speaker.Right) ? 1f : 0f;

        // Kalau None, gelapkan dua-duanya
        if (speaker == Speaker.None)
        {
            leftTarget = 0f;
            rightTarget = 0f;
        }

        if (leftFadeRoutine != null) StopCoroutine(leftFadeRoutine);
        if (rightFadeRoutine != null) StopCoroutine(rightFadeRoutine);

        leftFadeRoutine = StartCoroutine(LerpBrightness(leftMat, leftTarget));
        rightFadeRoutine = StartCoroutine(LerpBrightness(rightMat, rightTarget));
    }

    IEnumerator LerpBrightness(Material mat, float targetIsActive)
    {
        mat.SetFloat("_Saturation", 1f); // Pastikan warna tidak hilang

        float targetBrightness = (targetIsActive == 1f) ? 1f : inactiveBrightness;
        float currentBrightness = mat.GetFloat("_Brightness");

        while (!Mathf.Approximately(currentBrightness, targetBrightness))
        {
            currentBrightness = Mathf.MoveTowards(currentBrightness, targetBrightness, colorTransitionSpeed * Time.deltaTime);
            mat.SetFloat("_Brightness", currentBrightness);
            yield return null;
        }
    }
}