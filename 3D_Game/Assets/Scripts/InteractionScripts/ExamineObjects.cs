using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ExamineObjects : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private InventoryObjects itemData;
    [SerializeField] private bool requireDialogue = true;

    [Header("Inspection Settings")]
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float examineRange = 3f;
    [SerializeField] private float inspectDistance = 1.0f;

    [Header("Zoom & Offset Settings")]
    [SerializeField] private float minInspectDistance = 0.5f;
    [SerializeField] private float maxInspectDistance = 3.0f;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private Vector3 screenOffset = new Vector3(0f, -0.1f, 0f);

    [Header("UI Prompts")]
    [SerializeField] private GameObject inspectText;
    [SerializeField] private GameObject rotationText;
    [SerializeField] private GameObject zoomText;

    private bool isDialogueFinished = false;
    private bool isBeingInspected = false;
    private Rigidbody rb;
    private Collider col;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 localCenter;
    private float currentInspectDistance;

    private static ExamineObjects currentlyInspectedObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        if (itemData == null && GetComponent<InventoryObjects>() != null)
        {
            itemData = GetComponent<InventoryObjects>();
        }

        if (inspectText == null)
        {
            Text[] allTexts = Object.FindObjectsByType<Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (Text t in allTexts)
            {
                if (t.gameObject.name == "InspectText")
                {
                    inspectText = t.gameObject;
                    break;
                }
            }
        }

        if (rotationText == null)
        {
            Text[] allTexts = Object.FindObjectsByType<Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (Text t in allTexts)
            {
                if (t.gameObject.name == "RotationText")
                {
                    rotationText = t.gameObject;
                    break;
                }
            }
        }

        if (zoomText == null)
        {
            Text[] allTexts = Object.FindObjectsByType<Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (Text t in allTexts)
            {
                if (t.gameObject.name == "ZoomText")
                {
                    zoomText = t.gameObject;
                    break;
                }
            }
        }

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            localCenter = meshFilter.sharedMesh.bounds.center;
        }
        else
        {
            Collider itemCollider = GetComponent<Collider>();
            if (itemCollider != null)
            {
                localCenter = transform.InverseTransformPoint(itemCollider.bounds.center);
            }
            else
            {
                Renderer itemRenderer = GetComponent<Renderer>();
                if (itemRenderer != null)
                {
                    localCenter = transform.InverseTransformPoint(itemRenderer.bounds.center);
                }
                else
                {
                    localCenter = Vector3.zero;
                }
            }
        }

        if (inspectText != null) inspectText.SetActive(false);
        if (rotationText != null) rotationText.SetActive(false);
        if (zoomText != null) zoomText.SetActive(false);
    }

    void Update()
    {
        if (isBeingInspected)
        {
            Transform cam = Camera.main.transform;

            if (Mouse.current != null)
            {
                Vector2 scrollValue = Mouse.current.scroll.ReadValue();
                if (scrollValue.y != 0)
                {
                    float scrollDelta = Mathf.Clamp(scrollValue.y, -1f, 1f);
                    currentInspectDistance -= scrollDelta * zoomSpeed * 0.1f;
                    currentInspectDistance = Mathf.Clamp(currentInspectDistance, minInspectDistance, maxInspectDistance);
                }
            }

            Vector3 targetPosition = cam.position + (cam.forward * currentInspectDistance)
                                   + (cam.right * screenOffset.x)
                                   + (cam.up * screenOffset.y)
                                   + (cam.forward * screenOffset.z);
            transform.position = targetPosition;

            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                float xRotation = mouseDelta.x * rotationSpeed * Time.unscaledDeltaTime;
                float yRotation = mouseDelta.y * rotationSpeed * Time.unscaledDeltaTime;

                Vector3 worldCenter = transform.TransformPoint(localCenter);

                transform.RotateAround(worldCenter, cam.up, -xRotation);
                transform.RotateAround(worldCenter, cam.right, yRotation);
            }

            if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
            {
                EndInspection();
            }
        }
        else
        {
            if (currentlyInspectedObject == null)
            {
                if (rotationText != null && rotationText.activeSelf) rotationText.SetActive(false);
                if (zoomText != null && zoomText.activeSelf) zoomText.SetActive(false);
            }

            bool canInspect = (!requireDialogue || isDialogueFinished) && !ObjectDialogue.IsDialogueActive;

            if (canInspect)
            {
                bool isLookingAtItem = CheckIfLookingAtObject();

                if (isLookingAtItem)
                {
                    if (inspectText != null && !inspectText.activeSelf)
                    {
                        inspectText.SetActive(true);
                    }

                    if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                    {
                        if (inspectText != null) inspectText.SetActive(false);
                        StartInspection();
                    }
                }
                else
                {
                    if (inspectText != null && inspectText.activeSelf)
                    {
                        inspectText.SetActive(false);
                    }
                }
            }
            else
            {
                if (inspectText != null && inspectText.activeSelf)
                {
                    inspectText.SetActive(false);
                }
            }
        }
    }

    public void UnlockExamination()
    {
        isDialogueFinished = true;
    }

    private bool CheckIfLookingAtObject()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, examineRange))
        {
            return hit.collider.gameObject == gameObject;
        }
        return false;
    }

    public void StartInspection()
    {
        isBeingInspected = true;
        currentlyInspectedObject = this;
        currentInspectDistance = inspectDistance;

        if (inspectText != null) inspectText.SetActive(false);

        if (rotationText != null)
        {
            rotationText.SetActive(true);
            Text legacyText = rotationText.GetComponent<Text>();
            if (legacyText != null)
            {
                legacyText.enabled = false;
                legacyText.enabled = true;
                legacyText.SetAllDirty();
            }
        }

        if (zoomText != null)
        {
            zoomText.SetActive(true);
            Text legacyZoomText = zoomText.GetComponent<Text>();
            if (legacyZoomText != null)
            {
                legacyZoomText.enabled = false;
                legacyZoomText.enabled = true;
                legacyZoomText.SetAllDirty();
            }
        }

        Canvas.ForceUpdateCanvases();

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        if (col != null) col.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;

        InventoryManager manager = FindFirstObjectByType<InventoryManager>();
        if (manager != null && itemData != null)
        {
            manager.AddItem(itemData);
        }
    }

    public void EndInspection()
    {
        isBeingInspected = false;

        if (currentlyInspectedObject == this)
        {
            currentlyInspectedObject = null;
        }

        if (rotationText != null) rotationText.SetActive(false);
        if (zoomText != null) zoomText.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }

        if (col != null) col.enabled = true;
    }
}