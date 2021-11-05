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
    public int level = 1;
    public float maxHealth;
    public float health;
    public List<int> wallIds;
    public BuildingController buildingController;
    private UIController uiController;
    private float damageScale;
    private Renderer buildingRenderer;

    // Start is called before the first frame update
    void Start()
    {
        position = gameObject.transform.position;
        rotation = gameObject.transform.rotation;
        buildingController = FindObjectOfType<BuildingController>();
        uiController = buildingController.gameObject.GetComponent<UIController>();
        buildingRenderer = GetComponentInChildren<Renderer>();
        buildingRenderer.sharedMaterial.SetFloat("_PermamentDamageScale", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (damageScale > 0f)
        {
            buildingRenderer.sharedMaterial.SetFloat("_TemporaryDamageScale", damageScale);
            damageScale -= 0.05f;
        }
    }

    public void Destroy(bool first = true)
    {
        var controller = FindObjectOfType<GameController>();
        controller.buildings.Remove(this);
        if (first)
        {
            var walls = FindObjectsOfType<Building>();
            foreach (var wall in walls)
            {
                if (wall.type == Types.Wall || wall.type == Types.Gate)
                {
                    if (wallIds.Contains(wall.wallIds[0]))
                    {
                        wall.Destroy(false);
                    }
                }
                else if(wall!=this)
                {
                    foreach (var id in wallIds)
                    {
                        wall.wallIds.Remove(id);
                    }
                }
            }
        }
        GameObject.Destroy(gameObject);
    }

    public void Damage(float damage)
    {
        if (active)
        {
            health -= damage;
            if (health <= 0)
            {
                active = false;
                GameObject.Destroy(inside);
                inside = null;
                foreach (var renderer in GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }
                GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                buildingRenderer.sharedMaterial.SetFloat("_PermamentDamageScale", 1f - (health / maxHealth));
                damageScale = 1f;
            }
        }
    }

    public void Repair()
    {
        if (buildingController.RepairPay(type, level))
        {
            health += maxHealth / 10;
            if (!active)
            {
                active = true;
                foreach (var renderer in GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = true;
                }
                GetComponent<BoxCollider>().enabled = true;
            }
        }
    }

    public bool Upgrade()
    {
        if (uiController.upgradeMode && buildingController.Pay(type, level))
        {
            level++;
            SetStats();
            return true;
        }
        return false;
    }

    public void SetStats()
    {
        if(type == Types.Tower)
        {
            maxHealth = 100 * level;
            health = maxHealth;
        }
        else
        {
            maxHealth = 75 * level;
            health = maxHealth;
        }
    }
    public BuildingSave Serialize()
    {
        BuildingSave save = new BuildingSave();
        save.type = type;
        save.level = level;
        save.health = health;
        save.position = position;
        save.rotation = rotation.eulerAngles;
        if (inside)
            save.defence = inside.GetComponent<Defence>().Serialize();
        else
            save.defence = new DefenceSave();
        save.wallIds = wallIds;
        return save;
    }

    public void Select()
    {
        var children = GetComponentsInChildren<Transform>();
        foreach(var child in children)
        {
            if(child.gameObject.layer == LayerMask.NameToLayer("Buildings"))
                child.gameObject.layer = LayerMask.NameToLayer("Outline");
        }
    }

    public void Dselect()
    {
        var children = GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Outline"))
                child.gameObject.layer = LayerMask.NameToLayer("Buildings");
        }
    }
}
