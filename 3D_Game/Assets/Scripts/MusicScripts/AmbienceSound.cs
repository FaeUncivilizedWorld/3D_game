using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AmbienceSound : MonoBehaviour
{
    [Tooltip("Area of the sound to be in")]
    public Collider Area;
    [Tooltip("Character to track")]
    public GameObject Player;
    private static AudioSource audioSource;
    [SerializeField] private Slider ambienceSlider;

    void Update()
    {
        // Locate closest point on the collider to the player
        Vector3 closestPoint = Area.ClosestPoint(Player.transform.position);
        // Set position to closest point to the player
        transform.position = closestPoint;
    }

    //start is called before the first frame update
    void Start()
    {
        ambienceSlider.onValueChanged.AddListener(delegate { onValueChanged(); });
    }
    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void onValueChanged()
    {
        SetVolume(ambienceSlider.value);
    }
}