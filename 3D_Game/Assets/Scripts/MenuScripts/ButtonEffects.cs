using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    private Vector3 originalScale;
    private Vector3 targetScale;

    [Header("Scaling Settings")]
    [Tooltip("Size when hovering. 1.05 = 5% bigger")]
    public Vector3 hoverScale = new Vector3(1.05f, 1.05f, 1.05f);

    [Tooltip("Size when clicking. 0.95 = 5% smaller")]
    public Vector3 pressedScale = new Vector3(0.95f, 0.95f, 0.95f);

    public float smoothSpeed = 15f;

    void Start()
    {
        // Capture the true starting scale of the button
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // Smoothly interpolate scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * smoothSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = Vector3.Scale(originalScale, hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = Vector3.Scale(originalScale, pressedScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetScale = Vector3.Scale(originalScale, hoverScale);
    }
}