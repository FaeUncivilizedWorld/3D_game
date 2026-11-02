using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 40f;
    public Transform cameraHolder;

    public bool isExamining = false;

    private CharacterController characterController;
    private Vector3 velocity;
    private float XRotation = 0f;

    private bool playingFootsteps = false;
    public float footstepSpeed = 0.5f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PlayerPrefs.GetInt("LoadOnStart", 0) == 1)
        {
            LoadPlayerData();
            PlayerPrefs.SetInt("LoadOnStart", 0);
        }
    }

    void Update()
    {
        if (isExamining)
        {
            return;
        }

        if (Keyboard.current != null)
        {
            if (Keyboard.current.f5Key.wasPressedThisFrame)
            {
                SavePlayerData();
            }
            if (Keyboard.current.f9Key.wasPressedThisFrame)
            {
                LoadPlayerData();
            }
        }

        Look();
        Move();
    }

    void Look()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouseInput = Mouse.current.delta.ReadValue();

        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;

        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(XRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) { input.y += 1; }
            if (Keyboard.current.sKey.isPressed) { input.y -= 1; }
            if (Keyboard.current.aKey.isPressed) { input.x -= 1; }
            if (Keyboard.current.dKey.isPressed) { input.x += 1; }
        }

        Vector3 move = transform.right * input.x + transform.forward * input.y;
        if (move.magnitude > 1f)
            move.Normalize();

        characterController.Move(move * moveSpeed * Time.deltaTime);

        bool isMoving = input.magnitude > 0.1f && characterController.isGrounded;

        if (isMoving && !playingFootsteps)
        {
            StartFootsteps();
        }
        else if (!isMoving && playingFootsteps)
        {
            StopFootsteps();
        }

        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    public void DisableControlsForExamine()
    {
        isExamining = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EnableControlsAfterExamine()
    {
        isExamining = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SavePlayerData()
    {
        PlayerData data = SaveSystem.LoadPlayer() ?? new PlayerData();

        data.positionX = transform.position.x;
        data.positionY = transform.position.y;
        data.positionZ = transform.position.z;
        data.rotationY = transform.eulerAngles.y;

        data.savedObjects.Clear();
        SaveableObject[] allSaveableObjects = FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);

        foreach (SaveableObject obj in allSaveableObjects)
        {
            ObjectSaveData objData = new ObjectSaveData();
            objData.objectId = obj.objectId;

            objData.posX = obj.transform.position.x;
            objData.posY = obj.transform.position.y;
            objData.posZ = obj.transform.position.z;

            objData.rotX = obj.transform.eulerAngles.x;
            objData.rotY = obj.transform.eulerAngles.y;
            objData.rotZ = obj.transform.eulerAngles.z;

            Door doorScript = obj.GetComponent<Door>();
            if (doorScript != null)
            {
                objData.isOpen = doorScript.isOpen;
            }

            data.savedObjects.Add(objData);
        }

        SaveSystem.SavePlayer(data);
        Debug.Log("Game and World Objects Saved!");
    }

    public void LoadPlayerData()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            transform.position = new Vector3(data.positionX, data.positionY, data.positionZ);
            transform.rotation = Quaternion.Euler(0f, data.rotationY, 0f);

            if (controller != null) controller.enabled = true;

            SaveableObject[] allSaveableObjects = FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);

            foreach (SaveableObject obj in allSaveableObjects)
            {
                ObjectSaveData match = data.savedObjects.Find(x => x.objectId == obj.objectId);

                if (!string.IsNullOrEmpty(match.objectId))
                {
                    Rigidbody rb = obj.GetComponent<Rigidbody>();
                    if (rb != null) rb.isKinematic = true;

                    obj.transform.position = new Vector3(match.posX, match.posY, match.posZ);
                    obj.transform.rotation = Quaternion.Euler(match.rotX, match.rotY, match.rotZ);

                    if (rb != null) rb.isKinematic = false;

                    Door doorScript = obj.GetComponent<Door>();
                    if (doorScript != null)
                    {
                        doorScript.isOpen = match.isOpen;
                        if (doorScript.isOpen && doorScript.doorAnimator != null)
                        {
                            doorScript.doorAnimator.SetTrigger("OpenDoor");
                        }
                    }
                }
            }

            Debug.Log("Player and World Objects Loaded Successfully!");
        }
    }

    void StartFootsteps()
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootstep), 0f, footstepSpeed);
    }

    void StopFootsteps()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootstep));
    }

    void PlayFootstep()
    {
        SoundEffectManager.Play("Footsteps");
    }
}