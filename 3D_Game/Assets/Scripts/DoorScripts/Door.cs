using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    public Animator doorAnimator;
    public GameObject interactionText;
    private bool playerInRange = false;

    private void Start()
    {
        interactionText.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            doorAnimator.SetTrigger("OpenDoor");
            interactionText.SetActive(false);
            playerInRange = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionText.SetActive(false);
        }
    }
}
