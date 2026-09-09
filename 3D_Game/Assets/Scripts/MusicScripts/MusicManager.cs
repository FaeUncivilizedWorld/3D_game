using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSource;

    [Header("Scene Track Library")]
    public AudioClip BedroomMusic;
    public AudioClip AtticMusic;
    public AudioClip SequenceSceneMusic;
    public AudioClip WholeHouseMusic;

    [Header("Slider")]
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initial setup for the main menu volume slider
        SetupSliderListener();

        // Start playing the music for the very first scene
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // This automatically runs every time a new scene finishes loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play the music assigned to this scene
        PlayMusicForScene(scene.name);
        SetupSliderListener();
    }

    private void PlayMusicForScene(string sceneName)
    {
        Debug.Log("MusicManager is trying to load music for scene: " + sceneName);

        AudioClip selectedClip = null;
        switch (sceneName)
        {
            case "Bedroom":
                selectedClip = BedroomMusic;
                break;
            case "Attic":
                selectedClip = AtticMusic;
                break;
            case "SequenceScene":
                selectedClip = SequenceSceneMusic;
                break;
            case "WholeHouse":
                selectedClip = WholeHouseMusic;
                break;
        }

        // Play the track if it's different from what is already playing
        if (selectedClip != null && audioSource.clip != selectedClip)
        {
            audioSource.clip = selectedClip;
            audioSource.Play();
        }
    }

    private void SetupSliderListener()
    {
        // If the slider is missing (because we left the Main Menu), look for it in the new scene
        if (musicSlider == null)
        {
            musicSlider = FindFirstObjectByType<Slider>();
        }

        // If we found a slider, link it up and set its value to match the current volume
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveAllListeners(); // Clear old listeners to avoid errors
            musicSlider.value = audioSource.volume; // Make the slider match the saved volume
            musicSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
