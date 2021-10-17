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
    private DefenceController defenceController;

    // Start is called before the first frame update
    void Start()
    {
        defenceController = FindObjectOfType<DefenceController>();
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
        var barrel = GetComponent<Barrel>();
        if (barrel)
            barrel.Trigger();
    }

    public void Upgrade()
    {
        if(defenceController.Pay(type,level))
        {
            level++;
            SetStats();
        }
    }

    public void SetStats()
    {
        if(type == Defences.Archer)
        {
            var script = GetComponent<Turret>();
            script.damage = level * 5;
            script.fireRate = Mathf.Pow(1.2f, level);
            script.range = 10 * Mathf.Pow(1.2f, level);
        }
        if (type == Defences.Turret)
        {
            var script = GetComponent<Turret>();
            script.damage = level * 2;
            script.fireRate = 3 * Mathf.Pow(1.2f, level);
            script.range = 15 * Mathf.Pow(1.2f, level);
        }
        if (type == Defences.Barrel)
        {
            var script = GetComponent<Barrel>();
            script.damage = level * 50;
        }
    }

    public void Remove()
    {
        GetComponentInParent<Building>().inside = null;
    }
}
