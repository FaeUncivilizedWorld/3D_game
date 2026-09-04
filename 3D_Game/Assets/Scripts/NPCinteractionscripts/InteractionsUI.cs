using UnityEngine;
using TMPro; 

public class InteractionUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TextMeshProUGUI promptText;

    [Header("Settings")]
    [SerializeField] private string defaultKeyText = "[E]";

    private void OnEnable()
    {
        Interactor.OnTargetChange += UpdatePrompt;
    }

    private void OnDisable()
    {
        Interactor.OnTargetChange -= UpdatePrompt;
    }

    private void Start()
    {
        if (promptPanel != null) promptPanel.SetActive(false);
    }

    private void UpdatePrompt(string newPrompt)
    {
        if (promptPanel == null || promptText == null) return;

        if (string.IsNullOrEmpty(newPrompt))
        {
            promptPanel.SetActive(false);
        }
        else
        {
            promptPanel.SetActive(true);
            promptText.text = $"{defaultKeyText} {newPrompt}";
        }
    }
}