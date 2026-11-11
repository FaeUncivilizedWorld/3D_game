using UnityEngine;

public class MabelNPC : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject stairsToReveal; 
    [SerializeField] private GameObject climbTriggerToReveal; 

    public void FinishConversation()
    {
        if (stairsToReveal != null) stairsToReveal.SetActive(true);
        if (climbTriggerToReveal != null) climbTriggerToReveal.SetActive(true);

        Debug.Log("Conversation with Mabel ended. Stairs revealed!");
    }
}