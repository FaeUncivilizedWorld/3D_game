using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{

    public GameObject text;             

    private bool PlayerInZone;               

    public GameObject lightorobj;

    private void Start()
    {
        SoundEffectManager.Play("LightSwitch");
        PlayerInZone = false;                         
        text.SetActive(false);
    }

    private void Update()
    {
        if (PlayerInZone && Keyboard.current.fKey.wasPressedThisFrame)
        {
            lightorobj.SetActive(!lightorobj.activeSelf);
            gameObject.GetComponent<Animator>().Play("Switch");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")     
        {
            text.SetActive(true);
            PlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)     
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInZone = false;
            text.SetActive(false);
        }
    }
}