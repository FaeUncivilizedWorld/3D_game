using UnityEngine;
using UnityEngine.InputSystem;

public class PickupSystem : MonoBehaviour
{
    [Header("Settings")]
    public Transform cameraHolder;
    public Transform holdPoint;
    public float pickupDistance = 3f;
    public float moveSpeed = 15f;
    public LayerMask pickupLayer;

    private Rigidbody currentHeldObject;


    // Update is called once per frame
    void Update()
    {
        // Check for 'E' key press using the New Input System
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (currentHeldObject == null)
            {
                TryPickUp();
            }
            else
            {
                DropObject();
            }
        }
    }

    void FixedUpdate()
    {
        // Smoothly move the held object toward the HoldPoint using physics
        if (currentHeldObject != null)
        {
            Vector3 targetPosition = holdPoint.position;
            Vector3 direction = targetPosition - currentHeldObject.position;
            currentHeldObject.linearVelocity = direction * moveSpeed;
        }
    }


    void TryPickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraHolder.position, cameraHolder.forward, out hit, pickupDistance, pickupLayer))
        {
            if (hit.rigidbody != null)
            {
                currentHeldObject = hit.rigidbody;
                currentHeldObject.useGravity = false;
                currentHeldObject.freezeRotation = true; // Prevents wild spinning while carrying
            }
        }
    }

    void DropObject()
    {
        if (currentHeldObject != null)
        {
            currentHeldObject.useGravity = true;
            currentHeldObject.freezeRotation = false;
            currentHeldObject.linearVelocity = Vector3.zero; // Clear carried velocity
            currentHeldObject = null;
        }
    }
}
