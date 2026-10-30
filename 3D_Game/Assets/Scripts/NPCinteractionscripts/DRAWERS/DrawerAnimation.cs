
using UnityEngine;

public class CabinetAnimation : MonoBehaviour
{
    [Header("Cabinet Animation")]
    [SerializeField] private Animator cabinetAnimator;

    [SerializeField] private string animationTrigger = "OpenCabinet";

    public void OpenCabinet()
    {
        if (cabinetAnimator == null)
        {
            Debug.LogWarning("Cabinet Animator is not assigned!", this);
            return;
        }

        cabinetAnimator.ResetTrigger(animationTrigger);
        cabinetAnimator.SetTrigger(animationTrigger);
    }
}
