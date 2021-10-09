using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BuildingSave
{
    public Building.Types type;
    public int level;
    public float health;
    public Vector3 position;
    public Quaternion rotation;
    public DefenceSave defence;
}
