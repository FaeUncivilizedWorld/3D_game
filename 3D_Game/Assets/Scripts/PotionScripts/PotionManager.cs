using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PotionManager : MonoBehaviour
{
    public PotionGameManager miniGameManager;

    [Header("Ending Transition")]
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Recipe Board")]
    public GameObject recipeBoardPanel;
    public Image recipeSlot1;
    public Image recipeSlot2;
    public Image resultSlot;
    public Sprite questionMarkSprite;

    [Header("Score Counter")]
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        if (miniGameManager != null)
        {
            miniGameManager.OnScoreUpdated.AddListener(UpdateScoreDisplay);
            miniGameManager.OnMiniGameCompleted.AddListener(TriggerEndCutscene);
        }
    }

    public void ResetBoard()
    {
        recipeBoardPanel.SetActive(true);

        if (recipeSlot1 != null) { recipeSlot1.sprite = null; recipeSlot1.enabled = false; }
        if (recipeSlot2 != null) { recipeSlot2.sprite = null; recipeSlot2.enabled = false; }

        if (resultSlot != null)
        {
            if (questionMarkSprite != null)
            {
                resultSlot.sprite = questionMarkSprite;
                resultSlot.enabled = true;
            }
            else
            {
                resultSlot.sprite = null;
                resultSlot.enabled = false;
            }
        }
    }

    public void DisplaySelectedIngredient(PotionData potion, int slotNumber)
    {
        if (slotNumber == 1 && recipeSlot1 != null)
        {
            recipeSlot1.sprite = potion.potionIcon;
            recipeSlot1.enabled = true;
        }
        else if (slotNumber == 2 && recipeSlot2 != null)
        {
            recipeSlot2.sprite = potion.potionIcon;
            recipeSlot2.enabled = true;
        }
    }

    public void ShowResultPotion(PotionData resultPotion)
    {
        if (resultSlot != null && resultPotion != null)
        {
            resultSlot.sprite = resultPotion.potionIcon;
            resultSlot.enabled = true;
        }
    }

    public void UpdateScoreDisplay(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"{score}/4";
        }
    }

    public void TriggerEndCutscene()
    {
        StartCoroutine(FadeToEnding());
    }

    private IEnumerator FadeToEnding()
    {
        fadePanel.alpha = 0f;
        fadePanel.gameObject.SetActive(true);
        fadePanel.blocksRaycasts = true;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            fadePanel.alpha = Mathf.Clamp01(timer / fadeDuration);

            yield return null;
        }

        fadePanel.alpha = 1f;

        yield return new WaitForSecondsRealtime(0.3f);

        SceneManager.LoadScene("EndCutscene");
    }
}