using UnityEngine;

public class MabelNPC : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject stairsToReveal; // Drag your stairs GameObject here
    [SerializeField] private GameObject climbTriggerToReveal; // Drag your ClimbTrigger GameObject here

    // Call this method when the player finishes talking to Mabel
    public void FinishConversation()
    {
        if (stairsToReveal != null) stairsToReveal.SetActive(true);
        if (climbTriggerToReveal != null) climbTriggerToReveal.SetActive(true);

        Debug.Log("Conversation with Mabel ended. Stairs revealed!");
    }
}