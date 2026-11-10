using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "PotionSystem/Recipe")]
public class PotionRecipe : ScriptableObject
{
    public PotionData ingredient1;
    public PotionData ingredient2;
    public PotionData resultPotion; 
}