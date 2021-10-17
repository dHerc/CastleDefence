using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defence : MonoBehaviour
{
    public bool active = false;
    [Serializable]
    public enum Defences { None, Turret, Archer, Barrel };
    public Defences type;
    public int level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public DefenceSave Serialize()
    {
        DefenceSave save = new DefenceSave();
        save.type = type;
        save.level = level;
        return save;
    }

    public void Trigger()
    {
        Debug.Log("triggered");
    }

    public void Remove()
    {
        GetComponentInParent<Building>().inside = null;
    }
}
