using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("WholeHouse");
    }


    public void QuitGame()
    {
        Debug.Log("Game has been quit");
        Application.Quit();
    }

    public Button continueButton;
    public string gameSceneName = "GameScene";

    private void Start()
    {
        // Disables "Continue" button if no save data exists
        if (continueButton != null)
        {
            continueButton.interactable = SaveSystem.HasSavedData();
        }
    }

    public void OnClickNewGame()
    {
        PlayerPrefs.SetInt("LoadOnStart", 0);
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnClickContinue()
    {
        if (SaveSystem.HasSavedData())
        {
            // Set flag so PlayerController knows to load save data once loaded
            PlayerPrefs.SetInt("LoadOnStart", 1);
            SceneManager.LoadScene(gameSceneName);
        }
    }
}
