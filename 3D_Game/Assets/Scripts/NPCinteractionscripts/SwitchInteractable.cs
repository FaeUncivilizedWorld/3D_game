using UnityEngine;
using UnityEngine.Events;

public class SwitchInteractable : MonoBehaviour, IInteraction
{
  [SerializeField] private UnityEvent _onInteract; 
    UnityEvent IInteraction.OnInteract
    {
       get => _onInteract;
       set => _onInteract = value;
    }
    public void Interact() => _onInteract?.Invoke();

}

   

