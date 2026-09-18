using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro; 

public class IntroController : MonoBehaviour
{
    public GameObject clickHint;
    public Image fadeImage;

    [Header("Story Configurations")]
    public Image storyPage;
    // Array of sprites for the story pages, set in the inspector
    public Sprite[] pages;

    [TextArea(3, 5)]
    public string[] pageDialogues;
    public TextMeshProUGUI dialogueText;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip closetSound;

    // Called when the mouse is clicked
    private int currentPage = 0;
    private bool isTransitioning = false;

    [Header("Sway Settings")]
    public float swayAmount = 2.5f; // Amount of sway in degrees
    public float swaySpeed = 0.4f; // Speed of the sway

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.035f;

    [Header("Text Jiggle")]
    public float textJiggleAmount = 1.5f;
    public float textJiggleSpeed = 3f;

    private Coroutine typingCoroutine;
    private RectTransform dialogueRect;
    private Vector2 dialogueStartPos;

    // Reference to the RectTransform of the storyPage
    private RectTransform pageRect;
    private Vector2 startPos;

    void Start()
    {
        // Get the RectTransform component of the storyPage
        pageRect = storyPage.GetComponent<RectTransform>();
        startPos = pageRect.anchoredPosition;

        currentPage = 0;
        ShowPage();

        dialogueRect = dialogueText.GetComponent<RectTransform>();
        dialogueStartPos = dialogueRect.anchoredPosition;

        clickHint.SetActive(true);
        fadeImage.color = new Color(
            fadeImage.color.r,
            fadeImage.color.g,
            fadeImage.color.b,
            0
        ); 
    }
    void Update()
    {
        PageSway();
        TextJiggle();

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            !isTransitioning)
        {
            HandleClick();
        }
    }
    void TextJiggle()
    {
        if (dialogueRect == null)
            return;

        float x = Mathf.Sin(Time.time * textJiggleSpeed) * textJiggleAmount;
        float y = Mathf.Cos(Time.time * textJiggleSpeed * 0.8f) * textJiggleAmount;

        dialogueRect.anchoredPosition =
            dialogueStartPos + new Vector2(x, y);
    }
    void PageSway()
    {
        float t = Time.time * swaySpeed;
        float x = Mathf.Sin(t) * swayAmount;
        float y = Mathf.Cos(t * 0.9f) * swayAmount;
        // Apply the sway to the anchored position of the RectTransform
        pageRect.anchoredPosition = startPos + new Vector2(x, y);
    }

    void HandleClick()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;

            if (pageDialogues != null && currentPage < pageDialogues.Length)
            {
                dialogueText.text = pageDialogues[currentPage];
                dialogueText.maxVisibleCharacters = dialogueText.text.Length;
            }

            return;
        }

        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage();
            return;
        }

        StartCoroutine(EndSequence());
    }

    void ShowPage()
    {
        storyPage.sprite = pages[currentPage];

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogueText.text = "";

        if (pageDialogues != null && currentPage < pageDialogues.Length)
        {
            typingCoroutine = StartCoroutine(TypeDialogue(pageDialogues[currentPage]));
        }

        dialogueStartPos = dialogueRect.anchoredPosition;
    }
    IEnumerator TypeDialogue(string text)
    {
        dialogueText.text = text;
        dialogueText.maxVisibleCharacters = 0;

        for (int i = 0; i <= text.Length; i++)
        {
            dialogueText.maxVisibleCharacters = i;

            yield return new WaitForSeconds(typingSpeed);
        }

        typingCoroutine = null;
    }
    IEnumerator EndSequence()
    {
        isTransitioning = true;
        pageRect.anchoredPosition = startPos;
        clickHint.SetActive(false);

        // Hide the dialogue text during the final fade sequence
        if (dialogueText != null) dialogueText.text = "";

        yield return new WaitForSeconds(0.5f);

        if (audioSource != null && closetSound != null)
        {
            audioSource.PlayOneShot(closetSound);
        }

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(FadeOut());

        // FORCE FRAME RENDER BEFORE SCENE CHANGE
        yield return null;

        //SceneManager.LoadScene("VNScene");
    }

    IEnumerator FadeOut()
    {
        Debug.Log("Fade starting (Image method)");

        float duration = 3f;
        float t = 0f;

        Color c = fadeImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;

            float progress = t / duration;
            float alpha = Mathf.SmoothStep(0f, 1f, progress);

            fadeImage.color = new Color(c.r, c.g, c.b, alpha);

            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, 1f);
    }
}

