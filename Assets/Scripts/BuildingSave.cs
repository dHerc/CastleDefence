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
    public Vector3 rotation;
    public DefenceSave defence;
    public List<int> wallIds;
}
