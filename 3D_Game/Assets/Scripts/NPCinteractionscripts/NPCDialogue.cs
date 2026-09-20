using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("NPC Data")]
    [SerializeField] private string npcName = "name";

    [Header("Conversation Lines")]
    [SerializeField]
    [TextArea(3, 5)]
    private string[] dialogueLines = new string[]
    {
        "text",
        "text",
        "text"
    };

    private int _currentLineIndex = 0;
    private bool _conversationRunning = false;

    public void Speak()
    {
        if (_conversationRunning)
            return;

        if (dialogueLines == null || dialogueLines.Length == 0)
            return;

        _currentLineIndex = 0;
        _conversationRunning = true;

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (_currentLineIndex >= dialogueLines.Length)
        {
            EndConversation();
            return;
        }

        DialogueUI.Instance.DisplaySentence(
            npcName,
            dialogueLines[_currentLineIndex],
            OnLineFinished
        );
    }

    private void OnLineFinished()
    {
        _currentLineIndex++;

        if (_currentLineIndex >= dialogueLines.Length)
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

        DialogueUI.Instance.CloseDialogue();
    }
}
