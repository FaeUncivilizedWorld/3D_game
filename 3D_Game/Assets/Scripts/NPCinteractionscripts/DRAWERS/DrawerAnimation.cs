
using UnityEngine;

public class DrawerAnimation : MonoBehaviour
{
    [Header("Drawer Animation")]
    [SerializeField] private Animator drawerAnimator;

    [SerializeField] private string animationTrigger = "OpenDrawer";

    public void OpenDrawer()
    {
        if (drawerAnimator == null)
        {
            Debug.LogWarning("Drawer Animator is not assigned!", this);
            return;
        }

        drawerAnimator.ResetTrigger(animationTrigger);
        drawerAnimator.SetTrigger(animationTrigger);
    }
}
