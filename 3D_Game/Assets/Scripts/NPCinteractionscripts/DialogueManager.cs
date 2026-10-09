
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [Header("UI Canvas Elements")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI conversationText;

    [Header("Player Controls")]
    [SerializeField] private FPController playerController;

    [Header("Typing Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private Action onAdvance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!IsDialogueActive())
            return;

        bool advancePressed = false;

        if (Keyboard.current != null)
        {
            advancePressed =
                Keyboard.current.spaceKey.wasPressedThisFrame ||
                Keyboard.current.enterKey.wasPressedThisFrame;
        }

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            advancePressed = true;
        }

        if (advancePressed)
        {
            HandleAdvance();
        }
    }

    public void DisplaySentence(
        string speakerName,
        string text,
        Action onFinished)
    {
        dialoguePanel.SetActive(true);
        nameText.text = speakerName;
        onAdvance = onFinished;

        if (playerController != null)
        {
            playerController.DisableControlsForExamine();
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(text));
    }

    private IEnumerator TypeSentence(string text)
    {
        isTyping = true;

        conversationText.text = text;
        conversationText.ForceMeshUpdate();
        conversationText.maxVisibleCharacters = 0;

        int characterCount = conversationText.textInfo.characterCount;

        for (int i = 0; i < characterCount; i++)
        {
            conversationText.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(typingSpeed);
        }

        conversationText.maxVisibleCharacters = characterCount;

        isTyping = false;
        typingCoroutine = null;
    }

    private void HandleAdvance()
    {
        // First press: reveal the whole sentence.
        if (isTyping)
        {
            FinishTyping();
            return;
        }

        // Next press: advance to the next sentence.
        Action callback = onAdvance;
        onAdvance = null;
        callback?.Invoke();
    }

    private void FinishTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        conversationText.maxVisibleCharacters =
            conversationText.textInfo.characterCount;

        isTyping = false;
    }

    public void CloseDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;
        onAdvance = null;

        conversationText.text = "";
        conversationText.maxVisibleCharacters = 9999;

        dialoguePanel.SetActive(false);

        if (playerController != null)
        {
            playerController.EnableControlsAfterExamine();
        }
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

