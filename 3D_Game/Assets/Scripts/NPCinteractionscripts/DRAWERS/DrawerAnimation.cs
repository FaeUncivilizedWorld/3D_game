
using UnityEngine;

public class DrawerAnimation : MonoBehaviour
{
    [Header("Drawer Animation")]
    [SerializeField] private Animator drawerAnimator;

    [SerializeField] private string animationTrigger = "OpenDrawer";

    private bool hasOpened = false;
    public void OpenDrawer()
    {
        if (hasOpened)
            return;

        if (drawerAnimator == null)
        {
            Debug.LogWarning(
                "Drawer Animator is not assigned!",
                this
            );
            return;
        }

        drawerAnimator.SetTrigger(animationTrigger);
        hasOpened = true;
    }
}

