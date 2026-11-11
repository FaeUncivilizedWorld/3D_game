using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public static event Action<string> OnTargetChange;

    [Header("Interaction Settings")]
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableMask;

    private PlayerInput _playerInput;
    private IInteractable currentTarget;
    private FPController playerController; 

    public bool InputLocked { get; private set; }

    private void Awake()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
        playerController = GetComponentInParent<FPController>(); 
    }

    private void OnEnable()
    {
        if (_playerInput != null &&
            _playerInput.actions["Interact"] != null)
        {
            _playerInput.actions["Interact"].performed += OnInteractPressed;
        }
    }

    private void OnDisable()
    {
        if (_playerInput != null &&
            _playerInput.actions["Interact"] != null)
        {
            _playerInput.actions["Interact"].performed -= OnInteractPressed;
        }
    }

    private bool wasDialogueActiveLastFrame = false; 

    private void Update()
    {
        bool isDialogueActive = (DialogueUI.Instance != null && DialogueUI.Instance.IsDialogueActive());

        if (InputLocked || isDialogueActive)
        {
            if (isDialogueActive)
            {
                if (playerController != null && !playerController.isExamining)
                {
                    playerController.DisableControlsForExamine();
                }
                wasDialogueActiveLastFrame = true;
            }

            ClearCurrentTarget();
            return;
        }
        else
        {
            if (wasDialogueActiveLastFrame)
            {
                if (playerController != null && playerController.isExamining)
                {
                    playerController.EnableControlsAfterExamine();
                }
                wasDialogueActiveLastFrame = false;
            }
        }

        FindInteractable();
    }

    public void LockInput()
    {
        InputLocked = true;
        ClearCurrentTarget();
    }

    public void UnlockInput()
    {
        InputLocked = false;
    }

    private void ClearCurrentTarget()
    {
        if (currentTarget != null)
        {
            currentTarget = null;
            OnTargetChange?.Invoke(null);
        }
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
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                if (interactable != currentTarget)
                {
                    currentTarget = interactable;
                    OnTargetChange?.Invoke(currentTarget.InteractionPrompt);
                }

                return;
            }
        }

        ClearCurrentTarget();
    }

    private void OnInteractPressed(InputAction.CallbackContext context)
    {
        if (InputLocked)
            return;

        if (DialogueUI.Instance != null &&
            DialogueUI.Instance.IsDialogueActive())
            return;

        if (currentTarget != null)
        {
            bool success = currentTarget.Interact(this);

            if (success && currentTarget == null)
            {
                OnTargetChange?.Invoke(null);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (interactionPoint == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            interactionPoint.position +
            (interactionPoint.forward * interactionDistance),
            0.3f
        );
    }
}