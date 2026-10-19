using UnityEngine;

public class InventoryObjects : MonoBehaviour
{
    [Tooltip("The name of the object, as it will appear in the inventory menu UI.")]
    [SerializeField] private string objectName;

    [Tooltip("The text that will display when the player selects this object in the inventory menu.")]
    [TextArea(3, 10)]
    [SerializeField] private string description;

    [Tooltip("Icon sprite to display for this item in the inventory menu.")]
    [SerializeField] private Sprite icon;

    public string ObjectName => objectName;
    public string Description => description;
    public Sprite Icon => icon;

    private bool isCollected = false;

    public void Interact()
    {
        if (!isCollected)
        {
            isCollected = true;

            // Find the InventoryManager in the scene and add this item's info
            InventoryManager manager = FindObjectOfType<InventoryManager>();
            if (manager != null)
            {
                manager.AddItem(this);
            }
        }
    }
}