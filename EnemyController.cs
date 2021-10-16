using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject spawn;
    public GameObject warriorPrefab;
    public GameObject ramPrefab;
    public GameObject bomberPrefab;

    public int warriorCnt,ramCnt,bomberCnt;
    private List<GameObject> enemies;

    void Start()
    {
        enemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnEnemies()
    {
        var _warriorCnt=warriorCnt;
        var _ramCnt=ramCnt;
        var _bomberCnt=bomberCnt;

        float i=0;
        while(_warriorCnt!=0 && _ramCnt!=0 && _bomberCnt!=0)
        {
            if(_warriorCnt!=0)
            {
                GameObject enemy = Instantiate(warriorPrefab, new Vector3(spawn.transform.position.x + i ,spawn.transform.position.y,spawn.transform.position.z), spawn.transform.rotation);
                enemies.Add(enemy);
                i+=0.3f;
                _warriorCnt--;
            }
               
            if(_ramCnt!=0)
            {
                GameObject enemy =  Instantiate(ramPrefab, new Vector3(spawn.transform.position.x + i ,spawn.transform.position.y,spawn.transform.position.z), spawn.transform.rotation);
                enemies.Add(enemy);
                i+=0.3f;
                _ramCnt--;
            }
            if(_bomberCnt!=0)
            {
                GameObject enemy = Instantiate(bomberPrefab, new Vector3(spawn.transform.position.x + i,spawn.transform.position.y,spawn.transform.position.z), spawn.transform.rotation);
                enemies.Add(enemy);
                i+=0.3f;
                _bomberCnt--;
            }
        }
        
    }

    public void destroyEnemies()
    {
        foreach(var enemy in enemies)
        {
            GameObject.Destroy(enemy);
        }
    }
}
