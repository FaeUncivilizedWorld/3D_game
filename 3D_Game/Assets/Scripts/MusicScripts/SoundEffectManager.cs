using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager instance;

    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;

    [SerializeField] private Slider sfxSlider;

    [Header("Footstep Timing")]
    [SerializeField] private float footstepInterval = 0.4f;

    private static float lastFootstepTime = -Mathf.Infinity;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        if (soundEffectLibrary == null || audioSource == null)
            return;

        if (soundName == "Footsteps")
        {
            if (Time.time - lastFootstepTime < instance.footstepInterval)
                return;

            lastFootstepTime = Time.time;
        }

        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);

        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    void Start()
    {
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(delegate { onValueChanged(); });
        }
    }

    public static void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    public void onValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}
