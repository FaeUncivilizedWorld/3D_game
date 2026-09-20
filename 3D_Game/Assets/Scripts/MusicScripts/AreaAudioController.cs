using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AreaAudioController : MonoBehaviour
{
    public static AreaAudioController Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambienceSource;

    [Header("Volume Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambienceSlider;

    [Header("Fade Settings")]
    [SerializeField] private float musicFadeDuration = 2f;
    [SerializeField] private float ambienceFadeDuration = 3f;

    private AudioClip currentMusic;
    private AudioClip currentAmbience;

    private float musicVolume = 1f;
    private float ambienceVolume = 1f;

    private Coroutine musicFadeCoroutine;
    private Coroutine ambienceFadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        SetupSliders();
    }

    private void SetupSliders()
    {
        // MUSIC SLIDER
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveListener(SetMusicVolume);

            musicVolume = musicSlider.value;
            musicSource.volume = musicVolume;

            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        // AMBIENCE SLIDER
        if (ambienceSlider != null)
        {
            ambienceSlider.onValueChanged.RemoveListener(SetAmbienceVolume);

            ambienceVolume = ambienceSlider.value;
            ambienceSource.volume = ambienceVolume;

            ambienceSlider.onValueChanged.AddListener(SetAmbienceVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

        // Only change the volume directly if music isn't currently fading.
        if (musicFadeCoroutine == null)
        {
            musicSource.volume = musicVolume;
        }
    }

    public void SetAmbienceVolume(float volume)
    {
        ambienceVolume = volume;

        // Only change the volume directly if ambience isn't currently fading.
        if (ambienceFadeCoroutine == null)
        {
            ambienceSource.volume = ambienceVolume;
        }
    }

    public void ChangeAreaAudio(AudioClip newMusic, AudioClip newAmbience)
    {
        if (newMusic != currentMusic)
        {
            if (musicFadeCoroutine != null)
            {
                StopCoroutine(musicFadeCoroutine);
            }

            musicFadeCoroutine = StartCoroutine(
                CrossfadeMusic(newMusic)
            );
        }

        if (newAmbience != currentAmbience)
        {
            if (ambienceFadeCoroutine != null)
            {
                StopCoroutine(ambienceFadeCoroutine);
            }

            ambienceFadeCoroutine = StartCoroutine(
                CrossfadeAmbience(newAmbience)
            );
        }
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip)
    {
        float startingVolume = musicSource.volume;

        // Fade OUT current music
        if (musicSource.isPlaying)
        {
            float timer = 0f;

            while (timer < musicFadeDuration)
            {
                timer += Time.deltaTime;

                float progress = timer / musicFadeDuration;

                musicSource.volume = Mathf.Lerp(
                    startingVolume,
                    0f,
                    progress
                );

                yield return null;
            }
        }

        musicSource.volume = 0f;

        // Change the music
        currentMusic = newClip;

        if (newClip != null)
        {
            musicSource.clip = newClip;
            musicSource.loop = true;
            musicSource.Play();

            // Fade IN new music
            float timer = 0f;

            while (timer < musicFadeDuration)
            {
                timer += Time.deltaTime;

                float progress = timer / musicFadeDuration;

                musicSource.volume = Mathf.Lerp(
                    0f,
                    musicVolume,
                    progress
                );

                yield return null;
            }

            musicSource.volume = musicVolume;
        }

        musicFadeCoroutine = null;
    }

    private IEnumerator CrossfadeAmbience(AudioClip newClip)
    {
        float startingVolume = ambienceSource.volume;

        // Fade OUT current ambience
        if (ambienceSource.isPlaying)
        {
            float timer = 0f;

            while (timer < ambienceFadeDuration)
            {
                timer += Time.deltaTime;

                float progress = timer / ambienceFadeDuration;

                ambienceSource.volume = Mathf.Lerp(
                    startingVolume,
                    0f,
                    progress
                );

                yield return null;
            }
        }

        ambienceSource.volume = 0f;

        // Change the ambience
        currentAmbience = newClip;

        if (newClip != null)
        {
            ambienceSource.clip = newClip;
            ambienceSource.loop = true;
            ambienceSource.Play();

            // Fade IN new ambience
            float timer = 0f;

            while (timer < ambienceFadeDuration)
            {
                timer += Time.deltaTime;

                float progress = timer / ambienceFadeDuration;

                ambienceSource.volume = Mathf.Lerp(
                    0f,
                    ambienceVolume,
                    progress
                );

                yield return null;
            }

            ambienceSource.volume = ambienceVolume;
        }

        ambienceFadeCoroutine = null;
    }
}