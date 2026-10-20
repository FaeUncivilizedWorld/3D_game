
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class JournalUI : MonoBehaviour
{
    [Header("Journal UI")]
    [SerializeField] private GameObject journalCanvas;
    [SerializeField] private Image pageDisplay;

    [Header("Journal Pages")]
    [SerializeField] private Sprite[] pages;

    [Header("Player")]
    [SerializeField] private FPController playerController;

    private int currentPage = 0;
    private bool isOpen = false;

    private CursorLockMode previousCursorState;
    private bool previousCursorVisibility;

    private void Start()
    {
        journalCanvas.SetActive(false);
    }

    private void Update()
    {
        if (isOpen &&
            Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseJournal();
        }
    }

    public void OpenJournal()
    {
        SoundEffectManager.Play("PageFlip");

        if (isOpen || pages == null || pages.Length == 0)
            return;

        isOpen = true;
        currentPage = 0;

        previousCursorState = Cursor.lockState;
        previousCursorVisibility = Cursor.visible;

        if (playerController != null)
            playerController.DisableControlsForExamine();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        journalCanvas.SetActive(true);
        UpdatePage();
    }

    public void NextPage()
    {
        if (!isOpen || currentPage >= pages.Length - 1)
            return;

        currentPage++;
        UpdatePage();
    }

    public void PreviousPage()
    {
        if (!isOpen || currentPage <= 0)
            return;

        currentPage--;
        UpdatePage();
    }

    public void GoToPage(int pageNumber)
    {
        if (!isOpen || pageNumber < 1 ||
            pageNumber > pages.Length)
            return;

        currentPage = pageNumber - 1;
        UpdatePage();
    }

    private void UpdatePage()
    {
        pageDisplay.sprite = pages[currentPage];
        pageDisplay.enabled = pageDisplay.sprite != null;
    }

    public void CloseJournal()
    {
        if (!isOpen)
            return;

        isOpen = false;
        journalCanvas.SetActive(false);

        if (playerController != null)
            playerController.EnableControlsAfterExamine();

        Cursor.lockState = previousCursorState;
        Cursor.visible = previousCursorVisibility;
    }
}
