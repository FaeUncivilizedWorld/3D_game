using UnityEngine;
using UnityEngine.InputSystem; 

public class CodeTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AtticKeypad keypad; 
    [SerializeField] private GameObject codePromptText;

    private bool playerInZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            if (codePromptText != null)
                codePromptText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            if (codePromptText != null)
                codePromptText.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInZone && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (keypad != null)
            {
                keypad.OpenKeypad(); 
            }
        }
    }
}