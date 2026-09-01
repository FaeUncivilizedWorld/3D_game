using UnityEngine;
using UnityEngine.Events;

public class SwitchInteractable : MonoBehaviour, IInteraction
{
    private bool _isOn;
    [SerializeField] private UnityEvent _stopInteract;
    [SerializeField] private UnityEvent _onInteract; 
    UnityEvent IInteraction.OnInteract
    {
       get => _onInteract;
       set => _onInteract = value;
    }
    public void Interact()
    {
        if (_isOn)
            _stopInteract.Invoke();
        else
            _onInteract.Invoke();

        _isOn = !_isOn;
    }

}

   

