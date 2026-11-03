
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource sfxAudioSource;
    public AudioClip menuMusic;
    public AudioClip buttonSFX;

    [Header("Continue")]
    public Button continueButton;
    public string gameSceneName = "WholeHouse";

    private void Start()
    {
        // Load saved music volume
        if (audioSource != null && menuMusic != null)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            audioSource.Play();
        }

        // Disable Continue if no save data exists
        if (continueButton != null)
        {
            continueButton.interactable = SaveSystem.HasSavedData();
        }
    }

    public void NewGame()
    {
        StartCoroutine(NewGameWithSound());
    }

    private IEnumerator NewGameWithSound()
    {
        PlayButtonSound();

        if (buttonSFX != null)
            yield return new WaitForSeconds(buttonSFX.length);

        PlayerPrefs.SetInt("LoadOnStart", 0);
        PlayerPrefs.Save();

        // New Game goes to the sequence scene first
        SceneManager.LoadScene("SequenceScene");
    }

    public void QuitGame()
    {
        Debug.Log("The game is quit");
        Application.Quit();
    }

    public void OnClickNewGame()
    {
        NewGame();
    }

    public void OnClickContinue()
    {
        if (SaveSystem.HasSavedData())
        {
            StartCoroutine(ContinueWithSound());
        }
    }

    private IEnumerator ContinueWithSound()
    {
        PlayButtonSound();

        if (buttonSFX != null)
            yield return new WaitForSeconds(buttonSFX.length);

        PlayerPrefs.SetInt("LoadOnStart", 1);
        PlayerPrefs.Save();

        // Continue goes directly to WholeHouse
        SceneManager.LoadScene(gameSceneName);
    }

}


    private void PlayButtonSound()
    {
        AudioSource soundSource =
            sfxAudioSource != null ? sfxAudioSource : audioSource;

        if (soundSource != null && buttonSFX != null)
        {
            soundSource.PlayOneShot(buttonSFX);
        }
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

