using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save
{
    public int wave;
    public int maxWllId;
    public List<BuildingSave> buildings;
    public Save()
    {
        buildings = new List<BuildingSave>();
    }
}
