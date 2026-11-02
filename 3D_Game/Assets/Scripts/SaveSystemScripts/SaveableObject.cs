using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    [Tooltip("Must be completely unique for every saved object in your game!")]
    public string objectId;

    private void Awake()
    {
        if (string.IsNullOrEmpty(objectId))
        {
            Debug.LogWarning("SaveableObject on " + gameObject.name + " is missing an Object ID!", this);
        }
    }
}
