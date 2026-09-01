using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private LayerMask interactablelayer;
    private PlayerInput _playerInput;
    private Transform _transform;
    private void Awake()
    {
        _transform = transform;
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _playerInput.actions["Interact"].performed += DoInteract;
    }

    private void OnDisable()
    {
        _playerInput.actions["Interact"].performed -= DoInteract;
    }
    private void DoInteract(InputAction.CallbackContext callbackContext)
    {
        //raycasts 
        if (!Physics.Raycast(_transform.position + (Vector3.up * 0.3f) + (_transform.forward * 0.2f), 
            _transform.forward, out var hit, 1.5f, interactablelayer)) return;

        if(!hit.transform.TryGetComponent(out IInteraction interactable)) return;
        interactable.Interact();
        Debug.Log("Interact button pressed");
       
    }
   
}


