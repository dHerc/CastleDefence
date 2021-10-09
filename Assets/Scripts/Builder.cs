using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{

    public GameController controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Build(GameObject prefab, Vector3 position,Building.Types type, float health, float maxHealth, int level)
    {
        var building = Instantiate(prefab, position, Quaternion.identity);
        building.layer = 9;
        var data = building.GetComponent<Building>();
        data.maxHealth = maxHealth;
        data.health = health;
        data.type = type;
        data.level = level;
        data.active = true;
        controller.AddBuilding(data);
        return building;
    }
    public void AddDefence(GameObject prefab, GameObject parent, Defence.Defences type, int level)
    {
        var info = parent.GetComponent<Building>();
        info.inside = Instantiate(prefab, parent.transform);
        info.inside.GetComponent<Defence>().type = type;
        info.inside.GetComponent<Defence>().level = level;
    }
}
