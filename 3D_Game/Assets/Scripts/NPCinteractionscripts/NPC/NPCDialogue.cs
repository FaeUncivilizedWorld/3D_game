
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("NPC Data")]
    [SerializeField] private string npcName = "name";


    [Header("Attic Puzzle Reveal")]
    [SerializeField] private GameObject stairsToReveal;

    [Header("Player")]
    [SerializeField] private Interactor playerInteractor;


    [Header("First Conversation")]
    [SerializeField]
    [TextArea(3, 5)]

    private string[] firstDialogueLines =
    {
        "First dialogue.",
        "Another line."
    };

    [Header("Second Conversation")]
    [SerializeField]
    [TextArea(3, 5)]
    private string[] secondDialogueLines =
    {
        "Second dialogue.",
        "Another line."
    };

    private int _currentLineIndex = 0;
    private bool _conversationRunning = false;
    private int _conversationNumber = 0;

    private string[] _activeDialogue;

    public void Speak()
    {
        Debug.Log("Mabel Speak() called!");

        if (_conversationRunning)
            return;

        if (DialogueUI.Instance == null)
        {
            Debug.LogError("DialogueUI.Instance is missing in the scene!");
            return;
        }

        _activeDialogue = _conversationNumber == 0
            ? firstDialogueLines
            : secondDialogueLines;

        if (_activeDialogue == null || _activeDialogue.Length == 0)
        {
            Debug.LogWarning("No dialogue lines assigned in Inspector!");
            return;
        }

        _currentLineIndex = 0;
        _conversationRunning = true;
        if (playerInteractor != null)
            playerInteractor.LockInput();
        Debug.Log("NPC dialogue started. InputLocked: " + playerInteractor.InputLocked);

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (_currentLineIndex >= _activeDialogue.Length)
        {
            EndConversation();
            return;
        }

        DialogueUI.Instance.DisplaySentence(
            npcName,
            _activeDialogue[_currentLineIndex],
            OnLineAdvanced
        );
    }

    private void OnLineAdvanced()
    {
        _currentLineIndex++;

        if (_currentLineIndex >= _activeDialogue.Length)
        {
            EndConversation();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    private void EndConversation()
    {
        Debug.Log("EndConversation() REACHED!");

        _conversationRunning = false;
        _currentLineIndex = 0;

        _conversationNumber = (_conversationNumber == 0) ? 1 : 0;

        DialogueUI.Instance.CloseDialogue();

        if (playerInteractor != null)
            playerInteractor.UnlockInput();

        _activeDialogue = null;

        if (stairsToReveal != null)
        {
            stairsToReveal.SetActive(true);
            Debug.Log("Stairs revealed successfully!");
        }
        else
        {
            Debug.LogWarning("stairsToReveal is NOT required in John's inspector!");
        }
    }
}