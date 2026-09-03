using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("UI Settings")]
    [SerializeField] private string prompt = "Interact";
    public string InteractionPrompt => prompt;

    [Header("Events")]
    public UnityEvent onInteract;

    [Header("Settings")]
    public bool isSingleUse = false;
    private bool hasBeenUsed = false;

    public bool Interact(Interactor interactor)
    {
        if (isSingleUse && hasBeenUsed) return false;

        onInteract?.Invoke();

        if (isSingleUse)
        {
            hasBeenUsed = true;

            // Disable the collider so the SphereCast safely ignores it now
            if (TryGetComponent<Collider>(out var col)) col.enabled = false;

            // Destroy the object or hide its visuals
            Destroy(gameObject, 0.05f);
        }

        return true;
    }
}

