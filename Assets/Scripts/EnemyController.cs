using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject warriorPrefab;
    public GameObject ramPrefab;
    public GameObject bomberPrefab;

    private int warriorCnt,ramCnt,bomberCnt;
    public List<Enemy> enemies;
    private GameController controller;

    void Start()
    {
        enemies = new List<Enemy>();
        controller = gameObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isAttack && enemies.Count == 0)
            controller.EndWave();
            
    }

    private void SetEnemiesCount(int wave)
    {
        warriorCnt = (int)(Mathf.Pow(wave,1.2f) * 2);
        ramCnt = (int)(Mathf.Pow(wave/5, 1.2f) * 2);
        bomberCnt = (int)(Mathf.Pow(wave/10, 1.2f));
    }

    public void SpawnEnemies(int wave)
    {
        SetEnemiesCount(wave);
        for(int i=0; i<warriorCnt; i++)
        {
            SpawnEnemy(warriorPrefab, Enemy.Enemies.Warrior,wave*5,wave,Mathf.Ceil(wave/10f), new Vector3Int(wave,0,0));
        }
        for (int i = 0; i < ramCnt; i++)
        {
            SpawnEnemy(ramPrefab, Enemy.Enemies.Ram,wave*10,wave,Mathf.Ceil(wave/25f), new Vector3Int(0, wave*2, 0));
        }
        for (int i = 0; i < bomberCnt; i++)
        {
            SpawnEnemy(bomberPrefab, Enemy.Enemies.Bomber,wave*3,wave*20,Mathf.Ceil(wave/10f), new Vector3Int(0, 0, wave*3));
        }
    }

    private void SpawnEnemy(GameObject prefab, Enemy.Enemies type, float health, float damage, float speed, Vector3Int loot)
    {
        var side = Random.Range(1, 5);
        var placement = Random.Range(0.5f, 19.5f);
        Vector3 position;
        if(side==1||side==3)
            position = new Vector3(1 + 9*(side-1), 1, placement);
        else
            position = new Vector3(placement, 1, 1 + 9*(side-2));
        GameObject enemy = Instantiate(prefab, position, gameObject.transform.rotation);
        enemies.Add(enemy.GetComponent<Enemy>());
        var enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.enemyController = this;
        enemyScript.type = type;
        enemyScript.health = health;
        enemyScript.damage = damage;
        enemyScript.speed = speed;
        enemyScript.loot = loot;
        enemyScript.target = gameObject.transform;
    }

    public void AddLoot(Vector3Int loot)
    {
        controller.AddLoot(loot);
    }

    public void DestroyEnemies()
    {
        foreach(var enemy in enemies)
        {
            GameObject.Destroy(enemy.gameObject);
        }
    }
}
