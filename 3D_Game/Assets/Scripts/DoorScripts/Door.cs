using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    public Animator doorAnimator;
    public GameObject interactionText;
    private bool playerInRange = false;

    [Header("Door Save State")]
    public bool isOpen = false; 

    private void Start()
    {
        if (interactionText != null)
            interactionText.SetActive(false);

        if (isOpen && doorAnimator != null)
        {
            doorAnimator.SetTrigger("OpenDoor");
        }
    }

    private void Update()
    {
        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleDoor();
        }
    }

    public void ToggleDoor()
    {
        SoundEffectManager.Play("DoorOpen");

        if (!isOpen)
        {
            doorAnimator.SetTrigger("OpenDoor");
            isOpen = true;
        }
        else
        {
            doorAnimator.SetTrigger("CloseDoor");
            isOpen = false;
        }

        if (interactionText != null)
            interactionText.SetActive(false);

        playerInRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionText != null)
                interactionText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionText != null)
                interactionText.SetActive(false);
        }
    }
}