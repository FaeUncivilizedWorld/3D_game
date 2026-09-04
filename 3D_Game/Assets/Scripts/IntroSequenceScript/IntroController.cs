using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public GameObject clickHint;
    public Image fadeImage;

    public Image storyPage;
    // Array of sprites for the story pages, set in the inspector
    public Sprite[] pages;

    public AudioSource audioSource;
    public AudioClip closetSound;

    // Called when the mouse is clicked
    private int currentPage = 0;
    private bool isTransitioning = false;

    public float swayAmount = 2.5f; // Amount of sway in degrees
    public float swaySpeed = 0.4f; // Speed of the sway

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

        clickHint.SetActive(true);
        fadeImage.color = new Color(
            fadeImage.color.r,
            fadeImage.color.g,
            fadeImage.color.b,
            0
            ); // Ensure the fade image is initially transparent
    }

    void Update()
    {
        PageSway();

        // Check for mouse click to advance the story
        if (Input.GetMouseButtonDown(0) && !isTransitioning)
        {
            HandleClick();
        }
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

    }
    IEnumerator EndSequence()
    {
        isTransitioning = true;
        pageRect.anchoredPosition = startPos;
        clickHint.SetActive(false);

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
