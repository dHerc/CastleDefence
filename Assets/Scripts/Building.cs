using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private Vector3 position;
    private Quaternion rotation;
    public bool active = false;
    [Serializable]
    public enum Types { Tower, Wall, Gate};
    public Types type;
    public GameObject inside = null;
    public int level;
    public float maxHealth;
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        position = gameObject.transform.position;
        rotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            gameObject.transform.position = new Vector3(position.x, position.y - 0.5f + ((health+1)/(maxHealth*2)), position.z);
        }
    }

    public BuildingSave Serialize()
    {
        BuildingSave save = new BuildingSave();
        save.type = type;
        save.level = level;
        save.health = health;
        save.position = position;
        save.rotation = rotation;
        if (inside)
            save.defence = inside.GetComponent<Defence>().Serialize();
        else
            save.defence = new DefenceSave();
        return save;
    }
}
