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

    // This is the clean, original function your UnityEvent looks for
    public void Speak()
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        // Directly update the UI text using your manager
        DialogueUI.Instance.DisplaySentence(npcName, dialogueLines[_currentLineIndex]);

        // Advance to the next line
        _currentLineIndex++;

        // Loop back to the first line if we run out of sentences
        if (_currentLineIndex >= dialogueLines.Length)
        {
            _currentLineIndex = 0;
        }
    }
}
