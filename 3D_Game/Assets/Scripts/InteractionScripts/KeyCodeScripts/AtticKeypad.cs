using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AtticKeypad : MonoBehaviour
{
    [Header("Keycode Settings")]
    [SerializeField] private string correctCode = "10051970";
    private string currentInput = "";

    [Header("UI References")]
    [SerializeField] private GameObject keypadPanel;
    [SerializeField] private Text codeDisplayText;
    [SerializeField] private GameObject codePromptText;

    [Header("Climb & Zone")]
    [SerializeField] private GameObject climbTriggerObject;
    [SerializeField] private GameObject codeTriggerZone;

    [Header("Attic Reveal")]
    [SerializeField] private GameObject atticStairsObject;

    private bool playerInZone = false;

    public bool IsUnlocked { get; private set; } = false;

    void Start()
    {
        if (keypadPanel != null) keypadPanel.SetActive(false);
        if (codePromptText != null) codePromptText.SetActive(false);
    }

    void Update()
    {
        if (IsUnlocked) return;

        if (playerInZone && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && !keypadPanel.activeSelf)
        {
            OpenKeypad();
        }

        if (keypadPanel != null && keypadPanel.activeSelf)
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseKeypad();
            }

            HandleKeyboardTyping();
        }
    }

    public void OpenKeypad()
    {
        if (IsUnlocked) return;

        if (keypadPanel != null) keypadPanel.SetActive(true);

        FPController fp = FindFirstObjectByType<FPController>();
        if (fp != null) fp.DisableControlsForExamine();

        if (codePromptText != null)
            codePromptText.SetActive(false);

        ClearInput();
    }

    public void CloseKeypad()
    {
        if (keypadPanel != null) keypadPanel.SetActive(false);

        FPController fp = FindFirstObjectByType<FPController>();
        if (fp != null) fp.EnableControlsAfterExamine();

        if (playerInZone && !IsUnlocked && codePromptText != null)
        {
            codePromptText.SetActive(true);
        }

        ClearInput();
    }

    public void AddDigit(string digit)
    {
        if (currentInput.Length < 8)
        {
            currentInput += digit;
            UpdateDisplay();
        }

        if (currentInput.Length == 8)
        {
            CheckCode();
        }
    }

    public void ClearInput()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (codeDisplayText != null)
        {
            codeDisplayText.text = string.Join("        ", currentInput.ToCharArray());
        }
    }

    private void CheckCode()
    {
        if (currentInput == correctCode)
        {
            SoundEffectManager.Play("KeypadCorrect");

            Debug.Log("Attic Keycode Correct!");
            IsUnlocked = true;

            if (climbTriggerObject != null)
            {
                climbTriggerObject.SetActive(true);
            }

            if (codeTriggerZone != null)
            {
                codeTriggerZone.SetActive(false);
            }


            //  SoundEffectManager.Play(""); ANGIEE, ADD THE SOUND HERE


            CloseKeypad();

            if (codePromptText != null) codePromptText.SetActive(false);
        }
        else
        {
            Debug.Log("Incorrect Attic Keycode!");
            SoundEffectManager.Play("KeypadIncorrect");
            ClearInput();
        }
    }

    private void HandleKeyboardTyping()
    {
        if (Keyboard.current == null) return;

        for (int i = 0; i <= 9; i++)
        {
            Key key = (Key)((int)Key.Digit0 + i);
            Key numpadKey = (Key)((int)Key.Numpad0 + i);

            if (Keyboard.current[key].wasPressedThisFrame || Keyboard.current[numpadKey].wasPressedThisFrame)
            {
                AddDigit(i.ToString());
                break;
            }
        }

        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            ClearInput();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsUnlocked && (other.CompareTag("Player") || other.name.Contains("Player")))
        {
            playerInZone = true;
            if (codePromptText != null) codePromptText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.name.Contains("Player"))
        {
            playerInZone = false;
            if (codePromptText != null) codePromptText.SetActive(false);
            CloseKeypad();
        }
    }

    public void SetUnlockedStateFromSave()
    {
        IsUnlocked = true;

        // 1. Ensure visual stairs are visible
        if (atticStairsObject != null)
        {
            atticStairsObject.SetActive(true);
        }

        // 2. Turn on the climb/teleport trigger permanently
        if (climbTriggerObject != null)
        {
            climbTriggerObject.SetActive(true);
        }

        // 3. Turn off keypad trigger zone so prompt text never pops up
        if (codeTriggerZone != null)
        {
            codeTriggerZone.SetActive(false);
        }

        // 4. Ensure keypad UI panel is hidden
        if (keypadPanel != null)
        {
            keypadPanel.SetActive(false);
        }

        if (codePromptText != null)
        {
            codePromptText.SetActive(false);
        }
    }
}