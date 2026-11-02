using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    [Header("Save Settings")]
    [Tooltip("Type a completely unique ID for this switch")]
    [SerializeField] private string lightID;

    [Header("References")]
    [SerializeField] private GameObject text; // Your LightText object
    [SerializeField] private GameObject lightsParent; // Drag your "Lights" empty object here

    [Header("Behavior Settings")]
    [Tooltip("Check this if you want the lights to automatically turn ON the moment the player enters the zone.")]
    [SerializeField] private bool turnOnWhenEntering = false;

    private Light[] targetLights;
    private bool PlayerInZone;

    private void Start()
    {
        PlayerInZone = false;
        if (text != null) text.SetActive(false);

        // Automatically find all point lights inside the Lights parent object
        if (lightsParent != null)
        {
            targetLights = lightsParent.GetComponentsInChildren<Light>(true);
        }
        else
        {
            GameObject foundParent = GameObject.Find("Lights");
            if (foundParent != null)
            {
                targetLights = foundParent.GetComponentsInChildren<Light>(true);
            }
        }

        // Force lights to start OFF by default on game start
        SetLightsState(false);

        // Optional: Load saved state if you want persistence across game loads
        // LoadLightState();
    }

    private void Update()
    {
        if (PlayerInZone && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            ToggleLights();
        }
    }

    private void ToggleLights()
    {
        bool currentState = targetLights != null && targetLights.Length > 0 && targetLights[0].enabled;
        bool newState = !currentState;

        SetLightsState(newState);

        if (TryGetComponent<Animator>(out Animator anim))
        {
            anim.Play("Switch", 0, 0f);
        }

        SoundEffectManager.Play("LightSwitch");

        SaveLightState(newState);
    }

    private void SetLightsState(bool state)
    {
        if (targetLights == null || targetLights.Length == 0) return;

        foreach (Light l in targetLights)
        {
            if (l != null)
            {
                l.enabled = state;
            }
        }
    }

    private void SaveLightState(bool isOn)
    {
        PlayerData data = SaveSystem.LoadPlayer() ?? new PlayerData();

        if (data.lightsOnIds == null)
        {
            data.lightsOnIds = new List<string>();
        }

        if (isOn)
        {
            if (!data.lightsOnIds.Contains(lightID)) data.lightsOnIds.Add(lightID);
        }
        else
        {
            if (data.lightsOnIds.Contains(lightID)) data.lightsOnIds.Remove(lightID);
        }

        SaveSystem.SavePlayer(data);
    }

    private void LoadLightState()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null || data.lightsOnIds == null)
        {
            SetLightsState(false);
            return;
        }

        bool shouldBeOn = data.lightsOnIds.Contains(lightID);
        SetLightsState(shouldBeOn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.name.Contains("Player"))
        {
            if (text != null) text.SetActive(true);
            PlayerInZone = true;

            // If you want them to turn on automatically just by walking in:
            if (turnOnWhenEntering)
            {
                SetLightsState(true);
                if (TryGetComponent<Animator>(out Animator anim))
                {
                    anim.Play("Switch", 0, 0f);
                }
                SoundEffectManager.Play("LightSwitch");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.name.Contains("Player"))
        {
            PlayerInZone = false;
            if (text != null) text.SetActive(false);
        }
    }
}