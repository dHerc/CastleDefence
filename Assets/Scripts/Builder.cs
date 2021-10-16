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

    public GameObject Build(GameObject prefab, Vector3 position, Vector3 rotation, Building.Types type, float health, float maxHealth, int level)
    {
        var building = Instantiate(prefab, position, Quaternion.Euler(rotation));
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
        var size = parent.GetComponent<Collider>().bounds.size;
        var top = parent.transform.position.y + size.y / 2 + 0.1f;
        var position = new Vector3(parent.transform.position.x, top, parent.transform.position.z);
        info.inside = Instantiate(prefab, position, Quaternion.identity);
        info.inside.transform.parent = parent.transform;
        info.inside.GetComponent<Defence>().type = type;
        info.inside.GetComponent<Defence>().level = level;
    }
}
