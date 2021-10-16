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
            SpawnEnemy(warriorPrefab);
        }
        for (int i = 0; i < ramCnt; i++)
        {
            SpawnEnemy(ramPrefab);
        }
        for (int i = 0; i < bomberCnt; i++)
        {
            SpawnEnemy(bomberPrefab);
        }
    }

    private void SpawnEnemy(GameObject prefab)
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
        enemyScript.target = gameObject.transform;
    }

    public void DestroyEnemies()
    {
        foreach(var enemy in enemies)
        {
            GameObject.Destroy(enemy.gameObject);
        }
    }
}
