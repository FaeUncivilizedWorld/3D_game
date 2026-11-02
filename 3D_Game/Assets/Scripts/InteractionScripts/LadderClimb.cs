using UnityEngine;
using System.Collections;

public class ClimbTrigger : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private Transform targetDestination;

    private static bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            StartCoroutine(TeleportRoutine(other));
        }
    }

    private IEnumerator TeleportRoutine(Collider player)
    {
        isTeleporting = true;

        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        player.transform.position = targetDestination.position;

        if (controller != null) controller.enabled = true;

        yield return new WaitForSeconds(4f);

        isTeleporting = false;
    }
}