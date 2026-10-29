
using System.Collections;
using UnityEngine;

public class CabinetDoorInteraction : MonoBehaviour
{
    [Header("Sound Effect")]
    [SerializeField] private string soundEffectName = "CabinetStruggle";

    [Header("Dialogue")]
    [SerializeField] private ObjectDialogue objectDialogue;

    [SerializeField] private float dialogueDelay = 1.5f;

    private bool isPlaying = false;

    public void InteractWithObject()
    {
        if (isPlaying)
            return;

        StartCoroutine(PlaySoundAndDialogue());
    }

    private IEnumerator PlaySoundAndDialogue()
    {
        isPlaying = true;

        SoundEffectManager.Play(soundEffectName);

        yield return new WaitForSeconds(dialogueDelay);

        if (objectDialogue != null)
        {
            objectDialogue.Speak();
        }
        else
        {
            Debug.LogWarning(
                "ObjectDialogue is not assigned on " + gameObject.name,
                this
            );
        }

        isPlaying = false;
    }
}
