using UnityEngine;
using TMPro; // Ensure TextMeshPro is installed in your project

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [Header("UI Canvas Elements")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI conversationText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Hide dialogue display when the scene loads
        dialoguePanel.SetActive(false);
    }

    public void DisplaySentence(string speakerName, string text)
    {
        dialoguePanel.SetActive(true);
        nameText.text = speakerName;
        conversationText.text = text;

        // Auto-closes dialogue panel after 5 seconds of inactivity
        CancelInvoke(nameof(CloseDialogue));
        Invoke(nameof(CloseDialogue), 5f);
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
