using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Animator doorAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetTrigger("OpenDoor2");
            Debug.Log("TOUCH");
        }
    }
}
