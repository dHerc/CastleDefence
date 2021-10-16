using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private GameObject WallPrefab;
    [SerializeField] private GameObject TowerPrefab;
    [SerializeField] private GameObject GatePrefab;
    public int towerHealth;
    public int wallHealth;
    public float Size;

    private Vector3 target;
    private Vector3 position;

    private GameController controller;

    public int healthPerLevel;

    void Start()
    {
        controller = gameObject.GetComponent<GameController>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && !controller.isAttack)
        {
            target = hit.point;
            position.x = Mathf.Round(target.x / Size) * Size;
            position.y = target.y;
            position.z = Mathf.Round(target.z / Size) * Size;
            if (Input.GetMouseButtonDown(1))
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    hit.collider.gameObject.GetComponent<Building>().Destroy();
                }
                else
                {
                    controller.builder.Build(TowerPrefab, position, new Vector3(0,0,0), Building.Types.Tower, towerHealth, healthPerLevel, 1);
                }
                //wall.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                //wall.transform.position = new Vector3(wall.transform.position.x, 5, wall.transform.position.z);
            }
        }
    }
    public void Build(BuildingSave save)
    {
        GameObject building = null;
        if (save.type == Building.Types.Tower)
        {
            building = controller.builder.Build(TowerPrefab, save.position, save.rotation, Building.Types.Tower, save.health, save.level * healthPerLevel, save.level);
            building.GetComponent<Building>().wallIds = save.wallIds;
        }
        if (save.type == Building.Types.Wall)
        {
            building = controller.builder.Build(WallPrefab, save.position, save.rotation, Building.Types.Wall, save.health, save.level * healthPerLevel, save.level);
            building.GetComponent<Building>().wallIds = save.wallIds;
        }
        if (save.type == Building.Types.Gate)
        {
            building = controller.builder.Build(GatePrefab, save.position, save.rotation, Building.Types.Gate, save.health, save.level * healthPerLevel, save.level);
            building.GetComponent<Building>().wallIds = save.wallIds;
        }
        DefenceSave defence = save.defence;
        if(building != null && defence != null)
        {
            GetComponent<DefenceController>().AddDefence(defence, building);
        }

    }

    public void BuildWall(Vector3 position, Vector3 rotation)
    {
        controller.builder.Build(WallPrefab, position, rotation, Building.Types.Wall, healthPerLevel, healthPerLevel, 1).GetComponent<Building>().wallIds.Add(controller.maxWallId);
    }
}
