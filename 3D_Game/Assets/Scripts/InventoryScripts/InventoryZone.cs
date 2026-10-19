using UnityEngine;

public class InventoryZone : MonoBehaviour
{
    [Tooltip("Drag your 'Press I to Open Inventory' text object here.")]
    [SerializeField] private GameObject inventoryText;

    void Start()
    {
        if (inventoryText != null)
        {
            inventoryText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (inventoryText != null)
            {
                inventoryText.SetActive(true); 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (inventoryText != null)
            {
                inventoryText.SetActive(false); 
            }
        }
    }
}