using System;
using UnityEngine;
using UnityEngine.InputSystem; 

public class Interactor : MonoBehaviour
{
    // Events for the UI to listen to
    public static event Action<string> OnTargetChange;

    [Header("Interaction Settings")]
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableMask;

    private PlayerInput _playerInput;
    private IInteractable currentTarget;

    private void Awake()
    {
        // Finds PlayerInput on this object or its parent
        _playerInput = GetComponentInParent<PlayerInput>();
    }

    private void OnEnable()
    {
        if (_playerInput != null && _playerInput.actions["Interact"] != null)
        {
            _playerInput.actions["Interact"].performed += OnInteractPressed;
        }
    }

    private void OnDisable()
    {
        if (_playerInput != null && _playerInput.actions["Interact"] != null)
        {
            _playerInput.actions["Interact"].performed -= OnInteractPressed;
        }
    }
    private void Update()
    {
        if (DialogueUI.Instance != null && DialogueUI.Instance.IsDialogueActive())
        {
            if (currentTarget != null)
            {
                currentTarget = null;
                OnTargetChange?.Invoke(null);
            }

            return;
        }

        FindInteractable();
    }


    private void FindInteractable()
    {
        bool found = Physics.SphereCast(
            interactionPoint.position,
            0.3f,
            interactionPoint.forward,
            out RaycastHit hit,
            interactionDistance,
            interactableMask
        );

        if (found)
        {
            
            if (hit.collider.TryGetComponent(out IInteractable interactable))//||
               // hit.collider.GetComponentInParent<IInteractable>(out interactable)) 
            {
                if (interactable != currentTarget)
                {
                    currentTarget = interactable;
                    OnTargetChange?.Invoke(currentTarget.InteractionPrompt);
                }
                return;
            }
        }

        // Clear target if raycast hits nothing or a non-interactable object
        if (currentTarget != null)
        {
            currentTarget = null;
            OnTargetChange?.Invoke(null);
        }
    }

    private void OnInteractPressed(InputAction.CallbackContext context)
    {
        if (currentTarget != null)
        {
            bool success = currentTarget.Interact(this);

            // If the object was single-use and consumed, force-clear the UI immediately
            if (success && currentTarget == null)
            {
                OnTargetChange?.Invoke(null);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (interactionPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position + (interactionPoint.forward * interactionDistance), 0.3f);
    }
}
