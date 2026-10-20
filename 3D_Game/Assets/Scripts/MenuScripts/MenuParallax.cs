using UnityEngine;
using UnityEngine.InputSystem;

public class MenuParallax : MonoBehaviour
{
    [System.Serializable]
    public class Layer
    {
        public RectTransform transform;
        [Tooltip("How much this layer moves with the mouse. Lower = distant background, Higher = close foreground.")]
        public float multiplier = 10f;
        [HideInInspector] public Vector2 startPosition;
    }

    public Layer[] layers;
    public float smoothTime = 0.2f;

    private Vector2[] velocities;

    void Start()
    {
        velocities = new Vector2[layers.Length];

        foreach (var layer in layers)
        {
            if (layer.transform != null)
            {
                layer.startPosition = layer.transform.anchoredPosition;
            }
        }
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        Vector2 mouseViewport = Camera.main.ScreenToViewportPoint(mousePos);
        Vector2 centeredMouse = mouseViewport - new Vector2(0.5f, 0.5f);

        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].transform == null) continue;

            Vector2 targetPos = layers[i].startPosition + (centeredMouse * layers[i].multiplier);

            layers[i].transform.anchoredPosition = Vector2.SmoothDamp(
                layers[i].transform.anchoredPosition,
                targetPos,
                ref velocities[i],
                smoothTime
            );
        }
    }
}