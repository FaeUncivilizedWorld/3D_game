using UnityEngine;

public class InventoryObjects : MonoBehaviour
{
    [SerializeField] private string objectName;
    [TextArea(3, 10)]

    [SerializeField] private string description;

    [SerializeField] private Sprite icon;

    public string ObjectName => objectName;
    public string Description => description;
    public Sprite Icon => icon;

    private bool isCollected = false;

    [System.Obsolete]
    public void Interact()
    {
        if (!isCollected)
        {
            isCollected = true;

            InventoryManager manager = FindObjectOfType<InventoryManager>();
            if (manager != null)
            {
                manager.AddItem(this);
            }
        }
    }
}