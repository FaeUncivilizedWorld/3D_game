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
    public float mouseSensitivity = 100f;
    public Transform cameraHolder;

    [Header("Player Stats")]
    //This is where we write all the information that needs to be saved

    private CharacterController characterController;
    private Vector3 velocity;
    private float XRotation = 0f;

    public GameObject objectText;

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
            if (Keyboard.current.wKey.isPressed)
            {
                input.y += 1;
            }

            if (Keyboard.current.sKey.isPressed)
            {
                input.y -= 1;
            }

            if (Keyboard.current.aKey.isPressed)
            {
                input.x -= 1;
            }

            if (Keyboard.current.dKey.isPressed)
            {
                input.x += 1;
            }
        }

        Vector3 move = transform.right * input.x + transform.forward * input.y;
        if (move.magnitude > 1f)
            move.Normalize();

        characterController.Move(move * moveSpeed * Time.deltaTime);

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
    }
}
