using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI Panels & Prompts")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryText;
    [SerializeField] private Button exitButton;

    [Header("Inventory Item Slots")]
    [SerializeField] private Button[] slotButtons;

    [Header("Description Display")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;

    private List<InventoryObjects> collectedItems = new List<InventoryObjects>();



    void Start()
    {
        inventoryPanel.SetActive(false);

        exitButton.onClick.AddListener(CloseInventory);

        ClearDisplay();
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                if (inventoryPanel == null) return;

                if ((inventoryText != null && inventoryText.activeSelf) || inventoryPanel.activeSelf)
                {
                    ToggleInventory();
                }
            }
        }
    }

    public void AddItem(InventoryObjects newItem)
    {
        if (!collectedItems.Contains(newItem))
        {
            collectedItems.Add(newItem);
            UpdateInventoryUI();
        }
    }

    public void ToggleInventory()
    {
        bool isActive = inventoryPanel.activeSelf;

        if (!isActive)
        {
            OpenInventory();
        }
        else
        {
            CloseInventory();
        }
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;

        UpdateInventoryUI();
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            Image slotImage = slotButtons[i].GetComponent<Image>();

            if (i < collectedItems.Count)
            {
                slotButtons[i].gameObject.SetActive(true);
                InventoryObjects item = collectedItems[i];

                if (slotImage != null && item.Icon != null)
                {
                    slotImage.sprite = item.Icon;
                    slotImage.color = Color.white;
                }

                slotButtons[i].onClick.RemoveAllListeners();
                slotButtons[i].onClick.AddListener(() => DisplayItemDetails(item));
            }
            else
            {
                slotButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void DisplayItemDetails(InventoryObjects item)
    {
        itemNameText.text = item.ObjectName;
        itemDescriptionText.text = item.Description;
    }

    private void ClearDisplay()
    {
        itemNameText.text = "Item Name";
        itemDescriptionText.text = "Select an item to view its description.";

        foreach (Button slot in slotButtons)
        {
            slot.gameObject.SetActive(false);
        }
    }
}