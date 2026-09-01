using UnityEngine.Events;

public interface IInteraction
{
    public UnityEvent OnInteract { get; protected set; }
    public void Interact();
}

