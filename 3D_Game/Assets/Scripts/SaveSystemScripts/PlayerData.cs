using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct ObjectSaveData
{
    public string objectId;
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ;
    public bool isOpen;

}

[Serializable]
public class PlayerData
{
    // Player Stats & Position
    public float positionX;
    public float positionY;
    public float positionZ;
    public float rotationY;

  
    public List<ObjectSaveData> savedObjects = new List<ObjectSaveData>();
    public List<string> lightsOnIds = new List<string>();
}