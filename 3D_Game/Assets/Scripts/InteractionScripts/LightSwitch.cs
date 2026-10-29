using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    [Header("Save Settings")]
    [Tooltip("Type a completely unique ID for this switch, like 'Bedroom_Main_Lights'")]
    [SerializeField] private string lightID;

    [Header("References")]
    public GameObject text;
    public GameObject lightorobj; 

    private bool PlayerInZone;

    private void Start()
    {
        PlayerInZone = false;
        if (text != null) text.SetActive(false);

        LoadLightState();
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

        bool newState = !lightorobj.activeSelf;
        lightorobj.SetActive(newState);

        if (TryGetComponent<Animator>(out Animator anim))
        {
            anim.Play("Switch");
        }
        SoundEffectManager.Play("LightSwitch");

        SaveLightState(newState);
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
            if (!data.lightsOnIds.Contains(lightID))
            {
                data.lightsOnIds.Add(lightID);
            }
        }
        else
        {
            if (data.lightsOnIds.Contains(lightID))
            {
                data.lightsOnIds.Remove(lightID);
            }
        }

        SaveSystem.SavePlayer(data);
    }

    private void LoadLightState()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null || data.lightsOnIds == null)
        {
            lightorobj.SetActive(false);
            return;
        }

        bool shouldBeOn = data.lightsOnIds.Contains(lightID);
        lightorobj.SetActive(shouldBeOn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (text != null) text.SetActive(true);
            PlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInZone = false;
            if (text != null) text.SetActive(false);
        }
    }
}