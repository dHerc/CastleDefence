using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{   
    public EnemyController enemyController;
    public Text textOnButton;
    [SerializeField] private int wallHealth;
    private int wave = 1;
    [SerializeField] private float health = 1000;
    [SerializeField] private int gold = 100;
    [SerializeField] private int wood = 100;
    [SerializeField] private int stone = 100;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Text healthText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text woodText;
    [SerializeField] private Text stoneText;
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
        UpdateMaterials();
        UpdateHealth();
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

    public void Damage(float damage)
    {
        health -= damage;
        UpdateHealth();
        if(health <= 0)
        {
            //end game
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
        save.gold = gold;
        save.wood = wood;
        save.stone = stone;
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
        gold = save.gold;
        wood = save.wood;
        stone = save.stone;
        foreach(var building in save.buildings)
        {
            GetComponent<BuildingController>().Build(building);
        }
    }

    public void StartWave()
    {
        if(!isAttack)
        {
            shouldSave = true;
            isAttack = true;
            textOnButton.text="Fala nr" + wave;
            GetComponent<DefenceController>().DeselectTowers();
            GetComponent<EnemyController>().SpawnEnemies(wave);
            foreach (var building in buildings)
            {
                //it will turn off colliders for every destroyed building
                building.Damage(0);
            }

        }
    }

    public void AddLoot(Vector3Int loot)
    {
        gold += loot.x;
        wood += loot.y;
        stone += loot.z;
        UpdateMaterials();
    }

    public bool Pay(Vector3Int payment)
    {
        if (gold < payment.x)
            return false;
        if (wood < payment.y)
            return false;
        if (stone < payment.z)
            return false;
        gold -= payment.x;
        wood -= payment.y;
        stone -= payment.z;
        UpdateMaterials();
        return true;
    }

    public void EndWave()
    {
        wave++;
        AddLoot(new Vector3Int(10, 10, 10));
        shouldSave = true;
        isAttack = false;
        textOnButton.text = "Rozpocznij Szturm";
        GetComponent<EnemyController>().DestroyEnemies();
        foreach (var building in buildings)
        {
            building.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void UpdateHealth()
    {
        healthText.text = health.ToString();
        healthBar.value = health;
    }

    private void UpdateWood()
    {
        woodText.text = wood.ToString();
    }

    private void UpdateStone()
    {
        stoneText.text = stone.ToString();
    }

    private void UpdateGold()
    {
        goldText.text = gold.ToString();
    }

    private void UpdateMaterials()
    {
        UpdateWood();
        UpdateStone();
        UpdateGold();
    }
}
