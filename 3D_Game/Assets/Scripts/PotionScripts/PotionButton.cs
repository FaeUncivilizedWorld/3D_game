using UnityEngine;
using UnityEngine.UI;

public class PotionButton : MonoBehaviour
{
    public PotionData assignedPotion;
    private PotionGameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<PotionGameManager>();

        if (assignedPotion != null && GetComponent<Image>() != null)
        {
            GetComponent<Image>().sprite = assignedPotion.potionIcon;
        }
    }

    public void OnClick()
    {
        if (gameManager != null && assignedPotion != null)
        {
            gameManager.OnIngredientClicked(assignedPotion);
        }
    }
}