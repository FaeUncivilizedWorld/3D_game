using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    public string objectId;

    private void Awake()
    {
        if (string.IsNullOrEmpty(objectId))
        {
            Debug.LogWarning("SaveableObject on " + gameObject.name + " is missing an Object ID!", this);
        }
    }
}