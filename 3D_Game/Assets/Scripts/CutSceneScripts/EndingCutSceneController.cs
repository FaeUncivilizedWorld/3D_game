
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndingCutsceneController : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Screen Fade")]
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Ending Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip endingMusic;
    [SerializeField] private float musicFadeDuration = 2f;

    [Header("Scene Loading")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private float musicVolume = 1f;
    private float musicFadeMultiplier = 0f;

    private bool isEnding = false;

    private void OnEnable()
    {
        AreaAudioController.OnMusicVolumeChanged += UpdateMusicVolume;
    }

    private void OnDisable()
    {
        AreaAudioController.OnMusicVolumeChanged -= UpdateMusicVolume;

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }

    private void Start()
    {
        fadePanel.alpha = 1f;

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;

        videoPlayer.loopPointReached += OnVideoFinished;

        StartCoroutine(BeginCinematic());
    }

    private IEnumerator BeginCinematic()
    {
        // Prepare the video
        videoPlayer.Prepare();

        yield return new WaitUntil(() => videoPlayer.isPrepared);

        // Set up ending music
        if (musicSource != null && endingMusic != null)
        {
            musicSource.clip = endingMusic;
            musicSource.loop = true;

            musicFadeMultiplier = 0f;
            ApplyMusicVolume();

            musicSource.Play();
        }

        // Start video
        videoPlayer.Play();

        // Fade into the cinematic and fade in music
        StartCoroutine(FadeMusicIn());

        yield return StartCoroutine(FadeScreen(1f, 0f));
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        if (isEnding)
            return;

        isEnding = true;

        StopAllCoroutines();

        StartCoroutine(EndCinematic());
    }

    private IEnumerator EndCinematic()
    {
        float timer = 0f;

        float startingMusicVolume = musicFadeMultiplier;

        while (timer < Mathf.Max(fadeDuration, musicFadeDuration))
        {
            timer += Time.deltaTime;

            float screenProgress = fadeDuration > 0f
                ? Mathf.Clamp01(timer / fadeDuration)
                : 1f;

            float musicProgress = musicFadeDuration > 0f
                ? Mathf.Clamp01(timer / musicFadeDuration)
                : 1f;

            fadePanel.alpha = Mathf.Lerp(0f, 1f, screenProgress);

            musicFadeMultiplier = Mathf.Lerp(
                startingMusicVolume, 0f, musicProgress
            );

            ApplyMusicVolume();

            yield return null;
        }

        if (musicSource != null)
        {
            musicSource.Stop();
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator FadeScreen(float start, float end)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(timer / fadeDuration);

            fadePanel.alpha = Mathf.Lerp(start, end, progress);

            yield return null;
        }

        fadePanel.alpha = end;
    }

    private IEnumerator FadeMusicIn()
    {
        float timer = 0f;

        while (timer < musicFadeDuration)
        {
            timer += Time.deltaTime;

            musicFadeMultiplier = Mathf.Clamp01(
                timer / musicFadeDuration
            );

            ApplyMusicVolume();

            yield return null;
        }

        musicFadeMultiplier = 1f;
        ApplyMusicVolume();
    }

    private void UpdateMusicVolume(float volume)
    {
        musicVolume = volume;
        ApplyMusicVolume();
    }

    private void ApplyMusicVolume()
    {
        if (musicSource != null)
        {
            musicSource.volume =
                musicVolume * musicFadeMultiplier;
        }
    }
}
