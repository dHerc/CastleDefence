using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{   
    public EnemyController enemyController;
    public Text textOnButton;
    [SerializeField] private int wallHealth;
    private int wave;
    public bool isAttack;
    public List<Building> buildings;
    public Builder builder;
    public bool shouldSave;
    public bool shouldLoad;
    public int maxWallId = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<BuildingController>().towerHealth = wallHealth;
        gameObject.GetComponent<BuildingController>().wallHealth = wallHealth;
        builder = gameObject.GetComponent<Builder>();
        builder.controller = this;
        buildings = new List<Building>();
        isAttack=false;
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
        save.maxWllId = maxWallId;
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
        maxWallId = save.maxWllId;
        foreach(var building in save.buildings)
        {
            GetComponent<BuildingController>().Build(building);
        }
    }

    public void switchMode()
    {
        if(isAttack)
        {
            isAttack=false;
            textOnButton.text="Szturm";
            enemyController.destroyEnemies();

        }
        else
        {
            isAttack=true;
            textOnButton.text="Budowa";
            enemyController.spawnEnemies();
        }
    }
}
