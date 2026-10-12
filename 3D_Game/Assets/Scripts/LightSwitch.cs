using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour 
{

    public GameObject text;             //display the UI text

    private bool PlayerInZone;                  //check if the player is in trigger

    public GameObject lightorobj;

    private void Start()
    {

        PlayerInZone = false;                   //player not in zone       
        text.SetActive(false);
    }

    private void Update()
    {
        if (PlayerInZone && Input.GetKeyDown(KeyCode.E))           //if in zone and press E key
        {
            lightorobj.SetActive(!lightorobj.activeSelf);
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<Animator>().Play("Switch");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")     //if player in zone
        {
            text.SetActive(true);
            PlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)     //if player exit zone
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInZone = false;
            text.SetActive(false);
        }
    }
}
