
using System.Collections;
using UnityEngine;

public class CabinetDoorInteraction : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private string soundEffectName = "CabinetStruggle";
    [SerializeField] private InteractionUI interactionUI;

    [Header("Canvas Text Objects")]
    [SerializeField] private GameObject[] dialogueTexts;

    [Header("Player")]
    [SerializeField] private FPController playerController;

    [Header("Timing")]
    [SerializeField] private float dialogueDelay = 1.5f;
    [SerializeField] private float textDuration = 2f;

    private bool isPlaying;
    private bool wasControllerEnabled;

    private void Awake()
    {
        foreach (GameObject textObject in dialogueTexts)
        {
            if (textObject != null)
                textObject.SetActive(false);
        }
    }

    public void InteractWithObject()
    {
        if (isPlaying)
            return;

        StartCoroutine(PlaySoundAndDialogue());
    }

    private IEnumerator PlaySoundAndDialogue()
    {
        isPlaying = true;

        if (interactionUI != null)
        {
            interactionUI.ForceHidePrompt();
        }

        // Lock player movement and camera control.
        if (playerController != null)
        {
            wasControllerEnabled = playerController.enabled;
            playerController.enabled = false;
        }

        SoundEffectManager.Play(soundEffectName);

        yield return new WaitForSeconds(dialogueDelay);

        foreach (GameObject textObject in dialogueTexts)
        {
            if (textObject == null)
                continue;

            textObject.SetActive(true);

            yield return new WaitForSeconds(textDuration);

            textObject.SetActive(false);
        }

        // Restore player control.
        if (playerController != null)
            playerController.enabled = wasControllerEnabled;

        isPlaying = false;
    }

    private void OnDisable()
    {
        // Prevent the player from remaining locked if this object is disabled.
        if (playerController != null && isPlaying)
            playerController.enabled = wasControllerEnabled;

        StopAllCoroutines();
        isPlaying = false;

        foreach (GameObject textObject in dialogueTexts)
        {
            if (textObject != null)
                textObject.SetActive(false);
        }
    }
}
