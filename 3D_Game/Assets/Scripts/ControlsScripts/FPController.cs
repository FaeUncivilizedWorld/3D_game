using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

public class FPController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 75f;
    public Transform cameraHolder;
    public bool isExamining = false;

    [Header("Crouch")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 2.5f;

    private float originalMoveSpeed;

    //[Header("Player Stats")]
    //This is where we write all the information that needs to be saved

    private CharacterController characterController;
    private Vector3 velocity;
    private float XRotation = 0f;

    public GameObject objectText;

    private bool playingFootsteps = false;
    public float footstepSpeed = 0.5f; // 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        objectText.SetActive(false);

        // Check if we requested a load from the Main Menu or previous session
        if (PlayerPrefs.GetInt("LoadOnStart", 0) == 1)
        {
            LoadPlayerData();
            // Reset the flag so it doesn't force-load every scene reload unexpectedly
            PlayerPrefs.SetInt("LoadOnStart", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isExamining)
        {
            return;
        }

        Look();
        Move();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupObject"))
        {
            objectText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickupObject"))
        {
            objectText.SetActive(false);
        }
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

        // Checks if input keys are being pressed and the player is on the ground
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

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            characterController.height = crouchHeight;
            moveSpeed = crouchSpeed;
        }
        else if (context.canceled)
        {
            characterController.height = standHeight;
            moveSpeed = originalMoveSpeed;
        }
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
        PlayerData data = new PlayerData();

        // Save stats

        // Save position
        data.positionX = transform.position.x;
        data.positionY = transform.position.y;
        data.positionZ = transform.position.z;

        SaveSystem.SavePlayer(data);
    }

    public void LoadPlayerData()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            // If using a CharacterController or NavMeshAgent, disable it before changing transform.position
            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            transform.position = new Vector3(data.positionX, data.positionY, data.positionZ);

            if (controller != null) controller.enabled = true;

            Debug.Log("Player Loaded Successfully!");
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
