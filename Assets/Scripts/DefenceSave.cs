using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DefenceSave
{
    public Defence.Defences type;
    public int level;

    public DefenceSave() 
    {
        type = Defence.Defences.None;
        level = 0;
    }
}
