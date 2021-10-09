using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int wallHealth;
    private int wave;
    private List<Building> buildings;
    public Builder builder;
    public bool shouldSave;
    public bool shouldLoad;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<BuildingController>().towerHealth = wallHealth;
        gameObject.GetComponent<BuildingController>().wallHealth = wallHealth;
        builder = gameObject.GetComponent<Builder>();
        builder.controller = this;
        buildings = new List<Building>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldSave)
        {
            Save();
            shouldSave = false;
        }
        if (shouldLoad)
        {
            Load();
            shouldLoad = false;
        }
    }

    public void AddBuilding(Building building)
    {
        buildings.Add(building);
    }

    public void Save()
    {
        Save save = new Save();
        save.wave = wave;
        foreach(var building in buildings)
        {
            save.buildings.Add(building.Serialize());
        }
        var json = JsonUtility.ToJson(save);
        Debug.Log(json);
        PlayerPrefs.SetString("save", json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        var json = PlayerPrefs.GetString("save");
        Save save = JsonUtility.FromJson<Save>(json);
        wave = save.wave;
        foreach(var building in save.buildings)
        {
            GetComponent<BuildingController>().Build(building);
        }
    }
}
