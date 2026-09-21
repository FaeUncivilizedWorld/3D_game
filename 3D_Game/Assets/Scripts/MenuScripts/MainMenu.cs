using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip menuMusic;
    public AudioClip buttonSFX;

    [Header("Continue")]
    public Button continueButton;
    public string gameSceneName = "WholeHouse";

    private void Start()
    {
        // Start menu music
        if (audioSource != null && menuMusic != null)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.volume = 1f;
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
        if (audioSource != null && buttonSFX != null)
        {
            audioSource.PlayOneShot(buttonSFX);
            yield return new WaitForSeconds(buttonSFX.length);
        }

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
        if (audioSource != null && buttonSFX != null)
        {
            audioSource.PlayOneShot(buttonSFX);
            yield return new WaitForSeconds(buttonSFX.length);
        }

        // Tell PlayerController to load the saved data
        PlayerPrefs.SetInt("LoadOnStart", 1);
        PlayerPrefs.Save();

        // Continue goes directly to WholeHouse
        SceneManager.LoadScene(gameSceneName);
    }
}

