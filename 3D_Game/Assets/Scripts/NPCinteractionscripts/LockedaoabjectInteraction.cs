
using System.Collections;
using UnityEngine;

public class lockedObjectInteraction : MonoBehaviour
{
    [Header("Drawer Animation")]
    [SerializeField] private Animator drawerAnimator;
    [SerializeField] private string animationTrigger = "OpenDrawer";

    [Header("Refused Interaction")]
    [SerializeField] private string soundEffectName = "CabinetStruggle";

    [TextArea(2, 4)]
    [SerializeField]
    private string dialogueLine =
        "Hmmmm, I don't really need this right now.";

    [SerializeField] private float dialogueDelay = 1.5f;

    private bool isPlayingResponse = false;

    // Connect this method to InteractableObject's On Interact event.
    public void OpenDrawer()
    {
        if (drawerAnimator == null)
        {
            Debug.LogWarning("Drawer Animator is not assigned.", this);
            return;
        }

        drawerAnimator.SetTrigger(animationTrigger);
    }

    // Connect this method to cabinets and doors that cannot open.
    public void RefuseInteraction()
    {
        if (isPlayingResponse)
            return;

        StartCoroutine(PlayRefusal());
    }

    private IEnumerator PlayRefusal()
    {
        isPlayingResponse = true;

        // Play the struggling or handle-rattling sound.
        SoundEffectManager.Play(soundEffectName);

        // Wait before displaying the narrative dialogue.
        yield return new WaitForSeconds(dialogueDelay);

        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.DisplaySentence(
                "",
                dialogueLine,
                () =>
                {
                    DialogueUI.Instance.CloseDialogue();
                    isPlayingResponse = false;
                }
            );
        }
        else
        {
            Debug.LogWarning("DialogueUI instance was not found.", this);
            isPlayingResponse = false;
        }
    }
}

