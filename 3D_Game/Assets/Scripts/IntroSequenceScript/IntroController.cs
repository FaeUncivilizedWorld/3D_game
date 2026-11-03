
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class IntroController : MonoBehaviour
{
    [Header("Intro UI")]
    public GameObject clickHint;
    public Image storyPage;

    [Header("Scene Loading")]
    public LevelLoader levelLoader;

    [Header("Story Configurations")]
    public Sprite[] pages;

    [TextArea(3, 5)]
    public string[] pageDialogues;

    public TextMeshProUGUI dialogueText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource sfxAudioSource;
    public AudioClip introBGM;
    public AudioClip closetSound;

    [Header("Typewriter")]
    public float typingSpeed = 0.035f;

    [Header("Text Jiggle")]
    public float textJiggleAmount = 1.5f;
    public float textJiggleSpeed = 3f;

    [Header("Music Fade")]
    public float musicFadeDuration = 2f;

    private int currentPage = 0;
    private bool isTransitioning = false;

    private Coroutine typingCoroutine;

    private RectTransform dialogueRect;
    private Vector2 dialogueStartPosition;

    private void Start()
    {
        currentPage = 0;

        dialogueRect = dialogueText.GetComponent<RectTransform>();
        dialogueStartPosition = dialogueRect.anchoredPosition;

        ShowPage();

        clickHint.SetActive(true);

        // Load saved music volume
        if (audioSource != null && introBGM != null)
        {
            audioSource.clip = introBGM;
            audioSource.loop = true;
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            audioSource.Play();
        }
    }

    private void Update()
    {
        TextJiggle();

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            !isTransitioning)
        {
            HandleClick();
        }
    }

    private void TextJiggle()
    {
        if (dialogueRect == null)
            return;

        float x = Mathf.Sin(Time.time * textJiggleSpeed) * textJiggleAmount;
        float y = Mathf.Cos(Time.time * textJiggleSpeed * 0.8f) * textJiggleAmount;

        dialogueRect.anchoredPosition =
            dialogueStartPosition + new Vector2(x, y);
    }

    private void HandleClick()
    {
        // Finish typing if the text hasn't finished
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;

            if (pageDialogues != null &&
                currentPage < pageDialogues.Length)
            {
                dialogueText.text = pageDialogues[currentPage];
                dialogueText.maxVisibleCharacters =
                    dialogueText.text.Length;
            }

            return;
        }

        // Go to next page
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage();
            return;
        }

        // End intro
        StartCoroutine(EndSequence());
    }

    private void ShowPage()
    {
        storyPage.sprite = pages[currentPage];

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (pageDialogues != null &&
            currentPage < pageDialogues.Length)
        {
            typingCoroutine =
                StartCoroutine(TypeDialogue(pageDialogues[currentPage]));
        }
        else
        {
            dialogueText.text = "";
            dialogueText.maxVisibleCharacters = 0;
        }
    }

    private IEnumerator TypeDialogue(string text)
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

    private IEnumerator EndSequence()
    {
        isTransitioning = true;

        clickHint.SetActive(false);

        if (dialogueText != null)
        {
            dialogueText.text = "";
        }

        // Play closet sound
        AudioSource soundSource =
            sfxAudioSource != null ? sfxAudioSource : audioSource;

        if (soundSource != null && closetSound != null)
        {
            soundSource.PlayOneShot(closetSound);
        }

        yield return new WaitForSeconds(1f);

        // Fade out intro music
        yield return StartCoroutine(FadeMusicOut());

        levelLoader.LoadNextLevel();
    }

    private IEnumerator FadeMusicOut()
    {
        if (audioSource == null)
            yield break;

        float startingVolume = audioSource.volume;
        float timer = 0f;

        while (timer < musicFadeDuration)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(timer / musicFadeDuration);

            audioSource.volume =
                Mathf.Lerp(startingVolume, 0f, progress);

            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
    private void OnEnable()
    {
        AreaAudioController.OnMusicVolumeChanged += UpdateMusicVolume;
    }

    private void OnDisable()
    {
        AreaAudioController.OnMusicVolumeChanged -= UpdateMusicVolume;
    }

    private void UpdateMusicVolume(float volume)
    {
        if (audioSource != null)
            audioSource.volume = volume;
    }
}
