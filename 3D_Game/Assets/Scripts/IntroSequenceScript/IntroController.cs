
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroController : MonoBehaviour
{
    public Image storyPage;
    public Sprite[] pages;

    public CanvasGroup fadePanel;
    public GameObject clickHint;

    public AudioSource audioSource;
    public AudioClip DoorSound;

    private int currentPage = 0;
    private bool isTransitioning = false;

    void Start()
    {
        currentPage = 0;
        ShowPage();

        clickHint.SetActive(true);
        fadePanel.alpha = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTransitioning)
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        // If you're not on the last page go to next one
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage();
            return;
        }

        // If YOU'RE ON THE LAST PAGE start ending sequence
        StartCoroutine(EndSequence());
    }

    void ShowPage()
    {
        storyPage.sprite = pages[currentPage];
    }

    IEnumerator EndSequence()
    {
        isTransitioning = true;
        clickHint.SetActive(false);

        // Small tension pause before sound
        yield return new WaitForSeconds(0.5f);

        // Door sound moment
        if (audioSource != null && DoorSound != null)
        {
            audioSource.PlayOneShot(DoorSound);
        }

        // Wait for sound impact moment
        yield return new WaitForSeconds(1f);

        // Slow fade
        yield return StartCoroutine(FadeOut());

        //SceneManager.LoadScene("Bedroom");
    }
    IEnumerator FadeOut()
    {
        float duration = 3f; // SLOW fade 
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }

        fadePanel.alpha = 1;
    }
}