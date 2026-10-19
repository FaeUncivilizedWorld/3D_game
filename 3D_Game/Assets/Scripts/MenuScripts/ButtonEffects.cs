using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    private Vector3 originalScale;
    public Vector3 hoverScale = new(1.1f, 1.1f, 1.1f);

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = hoverScale;
    }
}