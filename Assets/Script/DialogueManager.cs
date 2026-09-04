using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public enum Speaker { Left, Right, None }

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 4)]
    public string text;
    public Speaker activeSpeaker; 
    public Sprite portraitOverride; 
}

public class DialogueManager : MonoBehaviour
{
    [Header("Portrait")]
    public Image leftPortraitImage;
    public Image rightPortraitImage;

    [Header("Wadah Teks (Kiri)")]
    public GameObject leftTextBubble;
    public TMP_Text leftNameText;
    public TMP_Text leftDialogueText;

    [Header("Wadah Teks (Kanan)")]
    public GameObject rightTextBubble;
    public TMP_Text rightNameText;
    public TMP_Text rightDialogueText;

    [Header("Wadah Teks (Narasi / None)")]
    public GameObject centerTextBubble; 
    public TMP_Text centerNameText; 
    public TMP_Text centerDialogueText;

    [Header("Pengaturan Teks")]
    public float typingSpeed = 0.03f;
    
    [Header("Pengaturan Dimming & Skala")]
    public float dimTransitionSpeed = 6f;
    [Range(0f, 1f)] public float inactiveBrightness = 0.3f; 
    public Vector3 activeScale = new Vector3(1.05f, 1.05f, 1f);
    public Vector3 inactiveScale = new Vector3(0.95f, 0.95f, 1f);

    [Header("Data Cerita")]
    public DialogueLine[] lines;

    [Header("Pengatur Transisi")]
    public VNToExploreManager vnManager;

    private int currentIndex = 0;
    private bool isTyping = false;
    private Coroutine typingRoutine;
    private Coroutine leftFadeRoutine;
    private Coroutine rightFadeRoutine;
    private TMP_Text currentDialogText;

    void Start()
    {
        leftTextBubble.SetActive(false);
        rightTextBubble.SetActive(false);
        if (centerTextBubble != null) centerTextBubble.SetActive(false);

        leftPortraitImage.rectTransform.localScale = inactiveScale;
        rightPortraitImage.rectTransform.localScale = inactiveScale;

        ShowLine(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping) CompleteTyping();
            else NextLine();
        }
    }

    void ShowLine(int index)
    {
        currentIndex = index;
        DialogueLine line = lines[index];

        leftTextBubble.SetActive(false);
        rightTextBubble.SetActive(false);
        if (centerTextBubble != null) centerTextBubble.SetActive(false);

        if (line.activeSpeaker == Speaker.Left)
        {
            rightPortraitImage.transform.SetAsFirstSibling();
            
            leftTextBubble.transform.SetAsLastSibling();
            leftPortraitImage.transform.SetAsLastSibling();
            
            leftTextBubble.SetActive(true);
            leftNameText.text = line.speakerName;
            currentDialogText = leftDialogueText;

            if (line.portraitOverride != null) leftPortraitImage.sprite = line.portraitOverride;
        }
        else if (line.activeSpeaker == Speaker.Right)
        {
            leftPortraitImage.transform.SetAsFirstSibling();
            
            rightTextBubble.transform.SetAsLastSibling();
            rightPortraitImage.transform.SetAsLastSibling();
            
            rightTextBubble.SetActive(true);
            rightNameText.text = line.speakerName;
            currentDialogText = rightDialogueText;

            if (line.portraitOverride != null) rightPortraitImage.sprite = line.portraitOverride;
        }
        else if (line.activeSpeaker == Speaker.None)
        {
            if (centerTextBubble != null)
            {
                centerTextBubble.transform.SetAsLastSibling();
                centerTextBubble.SetActive(true);
                if (centerNameText != null) centerNameText.text = line.speakerName;
                currentDialogText = centerDialogueText;
            }
        }

        SetActiveSpeaker(line.activeSpeaker);

        if (typingRoutine != null) StopCoroutine(typingRoutine);
        if (currentDialogText != null)
            typingRoutine = StartCoroutine(TypeText(line.text));
    }

    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        currentDialogText.text = "";
        foreach (char c in fullText)
        {
            currentDialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void CompleteTyping()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        if (currentDialogText != null) currentDialogText.text = lines[currentIndex].text;
        isTyping = false;
    }

    void NextLine()
    {
        int next = currentIndex + 1;
        if (next < lines.Length) ShowLine(next);
        else
        {
            leftTextBubble.SetActive(false);
            rightTextBubble.SetActive(false);
            if (centerTextBubble != null) centerTextBubble.SetActive(false);
            
            if (leftPortraitImage != null) leftPortraitImage.gameObject.SetActive(false);
            if (rightPortraitImage != null) rightPortraitImage.gameObject.SetActive(false);

            Debug.Log("Dialog selesai.");
            
            if (vnManager != null)
            {
                vnManager.SelesaiVN();
            }
        }
    }

    void SetActiveSpeaker(Speaker speaker)
    {
        float leftTargetBrightness = (speaker == Speaker.Left) ? 1f : inactiveBrightness;
        float rightTargetBrightness = (speaker == Speaker.Right) ? 1f : inactiveBrightness;

        Vector3 leftTargetScale = (speaker == Speaker.Left) ? activeScale : inactiveScale;
        Vector3 rightTargetScale = (speaker == Speaker.Right) ? activeScale : inactiveScale;

        if (speaker == Speaker.None)
        {
            leftTargetBrightness = inactiveBrightness;
            rightTargetBrightness = inactiveBrightness;
            leftTargetScale = inactiveScale;
            rightTargetScale = inactiveScale;
        }

        if (leftFadeRoutine != null) StopCoroutine(leftFadeRoutine);
        if (rightFadeRoutine != null) StopCoroutine(rightFadeRoutine);

        leftFadeRoutine = StartCoroutine(LerpColorAndScale(leftPortraitImage, leftTargetBrightness, leftTargetScale));
        rightFadeRoutine = StartCoroutine(LerpColorAndScale(rightPortraitImage, rightTargetBrightness, rightTargetScale));
    }

    IEnumerator LerpColorAndScale(Image portrait, float targetBrightness, Vector3 targetScale)
    {
        Color currentColor = portrait.color;
        Color targetColor = new Color(targetBrightness, targetBrightness, targetBrightness, 1f);
        Vector3 currentScale = portrait.rectTransform.localScale;

        while (Vector4.Distance(currentColor, targetColor) > 0.01f || Vector3.Distance(currentScale, targetScale) > 0.001f)
        {
            portrait.color = Color.Lerp(portrait.color, targetColor, Time.deltaTime * dimTransitionSpeed);
            portrait.rectTransform.localScale = Vector3.Lerp(portrait.rectTransform.localScale, targetScale, Time.deltaTime * dimTransitionSpeed);
            yield return null;
        }

        portrait.color = targetColor;
        portrait.rectTransform.localScale = targetScale;
    }
}