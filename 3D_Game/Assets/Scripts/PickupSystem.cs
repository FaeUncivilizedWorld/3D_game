using UnityEngine;
using UnityEngine.InputSystem;

public class PickupSystem : MonoBehaviour
{
    [Header("Settings")]
    public Transform cameraHolder;
    public Transform holdPoint;
    public float pickupDistance = 7f;
    public float moveSpeed = 15f;
    public LayerMask pickupLayer;
    public GameObject objectPanel;

    private Rigidbody currentHeldObject;

    void Start()
    {
        objectPanel.SetActive(false);
    }

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
                if (currentHeldObject != null)
                {
                    currentHeldObject.isKinematic = true;
                    currentHeldObject.useGravity = false;
                    currentHeldObject.linearVelocity = Vector3.zero;
                    currentHeldObject.angularVelocity = Vector3.zero;
                }

                if (holdPoint != null)
                {
                    transform.SetParent(holdPoint);
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.identity;
                }
                else
                {
                    Debug.LogError("Holdpoint is missing in the inspector on " + gameObject.name);
                }

                currentHeldObject = hit.rigidbody;
                currentHeldObject.useGravity = true;
               // currentHeldObject.freezeRotation = true; // Prevents wild spinning while carrying
                objectPanel.SetActive(true);

                ExamineObjects examine = FindAnyObjectByType<ExamineObjects>();
                if (examine != null)
                {
                    examine.StartExamining(this.transform);
                }
            }
        }
    }

    void DropObject()
    {
        transform.SetParent(null);

        if (currentHeldObject != null)
        {
            currentHeldObject.useGravity = true;
            currentHeldObject.isKinematic = false;
            currentHeldObject.freezeRotation = false;
            currentHeldObject.linearVelocity = Vector3.zero; // Clear carried velocity
            currentHeldObject = null;
            objectPanel.SetActive(false);

            ExamineObjects examine = FindAnyObjectByType<ExamineObjects>();
            if (examine != null)
            {
                examine.StopExamining();
            }
        }
    }
}
