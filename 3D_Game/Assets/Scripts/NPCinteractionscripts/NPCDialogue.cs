using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("NPC Data")]
    [SerializeField] private string npcName = "name";

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

    public void Speak()
    {
        if (_conversationRunning)
            return;

        string[] currentDialogue;

        if (_conversationNumber == 0)
        {
            currentDialogue = firstDialogueLines;
        }
        else
        {
            currentDialogue = secondDialogueLines;
        }

        if (currentDialogue == null || currentDialogue.Length == 0)
            return;

        _currentLineIndex = 0;
        _conversationRunning = true;

        ShowCurrentLine(currentDialogue);
    }

    private void ShowCurrentLine(string[] currentDialogue)
    {
        if (_currentLineIndex >= currentDialogue.Length)
        {
            EndConversation();
            return;
        }

        DialogueUI.Instance.DisplaySentence(
            npcName,
            currentDialogue[_currentLineIndex],
            () => OnLineFinished(currentDialogue)
        );
    }

    private void OnLineFinished(string[] currentDialogue)
    {
        _currentLineIndex++;

        if (_currentLineIndex >= currentDialogue.Length)
        {
            EndConversation();
        }
        else
        {
            ShowCurrentLine(currentDialogue);
        }
    }

    private void EndConversation()
    {
        _conversationRunning = false;
        _currentLineIndex = 0;

        // Switch to the other conversation
        if (_conversationNumber == 0)
        {
            _conversationNumber = 1;
        }
        else
        {
            _conversationNumber = 0;
        }

        DialogueUI.Instance.CloseDialogue();
    }
}
