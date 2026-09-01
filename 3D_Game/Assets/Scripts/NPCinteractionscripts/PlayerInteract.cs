using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private PlayerInput _playerInput;
    private void Awake()
    {
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
        Debug.Log("Interact button pressed");
        //raycasts 
    }
   
}


