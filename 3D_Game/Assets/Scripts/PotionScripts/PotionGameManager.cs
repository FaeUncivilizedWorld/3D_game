using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PotionGameManager : MonoBehaviour
{
    [Header("Recipes")]
    public List<PotionRecipe> allPossibleRecipes;

    [Header("Runtime State")]
    private List<PotionRecipe> activeRoundRecipes = new List<PotionRecipe>();
    private HashSet<PotionRecipe> completedRecipes = new HashSet<PotionRecipe>();
    private PotionData firstSelectedIngredient = null;
    private PotionData secondSelectedIngredient = null;
    private int successfulCombinationsCount = 0;

    [Header("Events")]
    public UnityEvent<int> OnScoreUpdated;
    public UnityEvent OnMiniGameCompleted;

    [Header("Panel & UI")]
    public GameObject potionGamePanel;
    public PotionManager potionUI;

    [SerializeField] private Interactor playerInteractor;

    [Header("Instructions Popup")]
    public GameObject instructionsPopup;

    public void StartMiniGame()
    {
        gameObject.SetActive(true);

        if (potionGamePanel != null)
        {
            potionGamePanel.SetActive(true);
        }

        FPController fp = FindFirstObjectByType<FPController>();
        if (fp != null)
        {
            fp.DisableControlsForExamine();
        }

        if (playerInteractor != null)
        {
            playerInteractor.LockInput();
        }
        SetupRound();

        if (instructionsPopup != null)
        {
            instructionsPopup.SetActive(true);
            StartCoroutine(HideInstructionsAfterDelay(15f));
        }
    }

    private IEnumerator HideInstructionsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (instructionsPopup != null)
        {
            instructionsPopup.SetActive(false);
        }
    }

    private void SetupRound()
    {
        successfulCombinationsCount = 0;
        completedRecipes.Clear();
        firstSelectedIngredient = null;
        secondSelectedIngredient = null;

        activeRoundRecipes = new List<PotionRecipe>(allPossibleRecipes);

        OnScoreUpdated?.Invoke(successfulCombinationsCount);
        if (potionUI != null) potionUI.ResetBoard();
    }

    public void OnIngredientClicked(PotionData clickedPotion)
    {
        if (clickedPotion == null) return;

        if (firstSelectedIngredient == null)
        {
            firstSelectedIngredient = clickedPotion;
            if (potionUI != null) potionUI.DisplaySelectedIngredient(clickedPotion, 1);
        }
        else if (secondSelectedIngredient == null && clickedPotion != firstSelectedIngredient)
        {
            secondSelectedIngredient = clickedPotion;
            if (potionUI != null) potionUI.DisplaySelectedIngredient(clickedPotion, 2);
        }
    }

    public void OnMixButtonClicked()
    {
        if (firstSelectedIngredient == null || secondSelectedIngredient == null)
        {
            Debug.Log("Select two ingredients first!");
            return;
        }

        PotionRecipe matchedRecipe = null;

        foreach (var recipe in activeRoundRecipes)
        {
            if (completedRecipes.Contains(recipe)) continue;

            bool isMatch = (recipe.ingredient1 == firstSelectedIngredient && recipe.ingredient2 == secondSelectedIngredient) ||
                           (recipe.ingredient1 == secondSelectedIngredient && recipe.ingredient2 == firstSelectedIngredient);
                            

            if (isMatch)
            {
                matchedRecipe = recipe;
                break;
            }
        }

        if (matchedRecipe != null)
        {
            SoundEffectManager.Play("PotionCorrect");

            Debug.Log("Correct combination!");
            completedRecipes.Add(matchedRecipe);

            if (potionUI != null)
            {
                potionUI.ShowResultPotion(matchedRecipe.resultPotion);
            }

            successfulCombinationsCount++;
            OnScoreUpdated?.Invoke(successfulCombinationsCount);

            if (successfulCombinationsCount >= 4)
            {
                Invoke("CompleteMiniGame", 1.5f);
            }
            else
            {
                Invoke("ResetSelectionSlots", 1.5f);
            }
        }
        else
        {
            SoundEffectManager.Play("PotionWrong");

            string ing1Name = firstSelectedIngredient != null ? firstSelectedIngredient.name : "NULL";
            string ing2Name = secondSelectedIngredient != null ? secondSelectedIngredient.name : "NULL";
            Debug.Log($"Wrong combination entered. Try again!");

            ResetSelectionSlots();
        }
    }

    private void ResetSelectionSlots()
    {
        firstSelectedIngredient = null;
        secondSelectedIngredient = null;
        if (potionUI != null)
        {
            potionUI.ResetBoard();
        }
    }

    private void CompleteMiniGame()
    {
        OnMiniGameCompleted?.Invoke();
    }
}