using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save
{
    public int wave;
    public int maxWllId;
    public int gold;
    public int wood;
    public int stone;
    public List<BuildingSave> buildings;
    public Save()
    {
        buildings = new List<BuildingSave>();
    }
}
