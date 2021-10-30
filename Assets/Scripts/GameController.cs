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
    private float health = 100;
    [SerializeField] private int gold = 1000;
    [SerializeField] private int wood = 1000;
    [SerializeField] private int stone = 1000;
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
    public Image gameOverImage;
    public Text gameOverText;
    private float damageScale;
    private Renderer castleRenderer;

    public float Health { get => health; set { health = value; healthText.text = value.ToString(); healthBar.value = value; } }

    public int Gold { get => gold; set { gold = value; goldText.text = value.ToString(); } }
    public int Wood { get => wood; set { wood = value; woodText.text = value.ToString(); } }
    public int Stone { get => stone; set { stone = value; stoneText.text = value.ToString(); } }

    public 
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<BuildingController>().towerHealth = wallHealth;
        gameObject.GetComponent<BuildingController>().wallHealth = wallHealth;
        builder = gameObject.GetComponent<Builder>();
        builder.controller = this;
        buildings = new List<Building>();
        isAttack=false;
        healthText.text = health.ToString();
        goldText.text = gold.ToString();
        woodText.text = wood.ToString();
        stoneText.text = stone.ToString();
        gameOverImage.enabled = false;
        gameOverText.enabled = false;
        castleRenderer = GetComponentInChildren<Renderer>();
        castleRenderer.sharedMaterial.SetFloat("_PermamentDamageScale", 0);
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
        if(damageScale > 0f)
        {
            castleRenderer.sharedMaterial.SetFloat("_TemporaryDamageScale", damageScale);
            damageScale -= 0.05f;
        }
    }

    public void Damage(float damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            gameOverImage.enabled = true;
            gameOverText.enabled = true;
        }
        else
        {
            castleRenderer.sharedMaterial.SetFloat("_PermamentDamageScale", 1f - (health / 100));
            damageScale = 1f;
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
        save.gold = Gold;
        save.wood = Wood;
        save.stone = Stone;
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
        Gold = save.gold;
        Wood = save.wood;
        Stone = save.stone;
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
        Gold += loot.x;
        Wood += loot.y;
        Stone += loot.z;
    }

    public bool Pay(Vector3Int payment)
    {
        if (Gold < payment.x)
            return false;
        if (Wood < payment.y)
            return false;
        if (Stone < payment.z)
            return false;
        Gold -= payment.x;
        Wood -= payment.y;
        Stone -= payment.z;
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
}
