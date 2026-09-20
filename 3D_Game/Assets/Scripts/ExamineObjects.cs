using UnityEngine;
using UnityEngine.InputSystem;

public class ExamineObjects : MonoBehaviour
{
    [Header("References")]
    public FPController controller;
    public Transform examinePoint;

    private Transform currentExamineObject;
    private bool isExamining = false;

    void Update()
    {
        if (isExamining && currentExamineObject != null)
        {
            RotateExamineObject();
        }
    }

    public void StartExamining(Transform targetObject)
    {
        isExamining = true;
        currentExamineObject = targetObject;

        // Position object at the examine point in front of camera
        currentExamineObject.position = examinePoint.position;
        currentExamineObject.SetParent(examinePoint);

        // Pause player look & movement
        if (controller != null)
        {
            controller.DisableControlsForExamine();
        }
    }

    public void StopExamining()
    {
        isExamining = false;

        if (currentExamineObject != null)
        {
            currentExamineObject.SetParent(null);
            currentExamineObject = null;
        }

        // Resume player look & movement
        if (controller != null)
        {
            controller.EnableControlsAfterExamine();
        }
    }

    private void RotateExamineObject()
    {
        // Rotate the examined object using mouse drag or stick input
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            currentExamineObject.Rotate(Vector3.up, -mouseDelta.x * 0.2f, Space.World);
            currentExamineObject.Rotate(Vector3.right, mouseDelta.y * 0.2f, Space.World);
        }
    }
}

