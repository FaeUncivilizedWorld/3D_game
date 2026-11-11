using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject hud;
    [SerializeField] private Interactor playerInteractor;

    private bool isPaused = false;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerInteractor != null)
        {
            playerInteractor.LockInput();
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerInteractor != null)
        {
            playerInteractor.UnlockInput();
        }
    }

    public void Menu(int sceneID)
    {
        Time.timeScale = 1f;

        if (playerInteractor != null)
        {
            playerInteractor.UnlockInput();
        }

        SceneManager.LoadScene(sceneID);
    }
}
