
using UnityEngine;

public class ObjectInteractionHint : MonoBehaviour
{
    [Header("Interaction Hint")]
    [SerializeField] private GameObject hintText;
    [SerializeField] private Transform player;
    [SerializeField] private float showDistance = 3f;

    private void Start()
    {
        if (hintText != null)
        {
            hintText.SetActive(false);
        }

        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void Update()
    {
        if (hintText == null ||
            player == null ||
            DialogueUI.Instance == null)
        {
            HideHint();
            return;
        }

        bool dialogueActive = DialogueUI.Instance.IsDialogueActive();

        float distance =
            Vector3.Distance(player.position, transform.position);

        bool showHint = dialogueActive && distance <= showDistance;

        hintText.SetActive(showHint);
    }

    private void HideHint()
    {
        if (hintText != null && hintText.activeSelf)
        {
            hintText.SetActive(false);
        }
    }
}
