
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("NPC Data")]
    [SerializeField] private string npcName = "name";

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
        if (_conversationRunning)
            return;

        if (DialogueUI.Instance == null)
            return;

        _activeDialogue = _conversationNumber == 0
            ? firstDialogueLines
            : secondDialogueLines;

        if (_activeDialogue == null || _activeDialogue.Length == 0)
            return;

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
        _conversationRunning = false;
        _currentLineIndex = 0;

        // Alternate between the two conversations.
        _conversationNumber = (_conversationNumber == 0) ? 1 : 0;

        DialogueUI.Instance.CloseDialogue();

        if (playerInteractor != null)
            playerInteractor.UnlockInput();

        _activeDialogue = null;
    }
}
