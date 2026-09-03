using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("NPC Data")]
    [SerializeField] private string npcName = "Mabel";

    [SerializeField]
    [TextArea(3, 5)]
    private string dialogueLine = "Ethy?";

    // This public method will be called via your UnityEvents inspector!
    public void Speak()
    {
        DialogueUI.Instance.DisplaySentence(npcName, dialogueLine);
    }
}