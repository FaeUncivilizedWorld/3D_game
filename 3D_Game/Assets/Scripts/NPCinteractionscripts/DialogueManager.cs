using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [Header("UI Canvas Elements")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI conversationText;

    [Header("Typing Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        dialoguePanel.SetActive(false);
    }

    public void DisplaySentence(string speakerName, string text, System.Action onFinished)
    {
        dialoguePanel.SetActive(true);

        nameText.text = speakerName;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(text, onFinished));
    }

    private IEnumerator TypeSentence(string text, System.Action onFinished)
    {
        isTyping = true;

        conversationText.text = text;
        conversationText.maxVisibleCharacters = 0;

        for (int i = 0; i <= text.Length; i++)
        {
            conversationText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;

        onFinished?.Invoke();
    }
    public void CloseDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;
        conversationText.text = "";
        conversationText.maxVisibleCharacters = 9999;
        dialoguePanel.SetActive(false);
    }

    public bool IsDialogueActive()
    {
        return dialoguePanel != null && dialoguePanel.activeSelf;
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}
