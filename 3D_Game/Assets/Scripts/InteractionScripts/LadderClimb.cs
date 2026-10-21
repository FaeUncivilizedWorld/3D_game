using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private Transform targetDestination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;

            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = targetDestination.position;
            player.transform.rotation = targetDestination.rotation;

            if (cc != null) cc.enabled = true;
        }
    }
}