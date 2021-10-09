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

    void Start()
    {
        controller = gameObject.GetComponent<GameController>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            target = hit.point;
            position.x = Mathf.Round(target.x / Size) * Size;
            position.y = target.y;
            position.z = Mathf.Round(target.z / Size) * Size;
            if (Input.GetMouseButtonDown(1))
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    Destroy(hit.collider.gameObject);
                }
                else
                {
                    controller.builder.Build(TowerPrefab, position, Building.Types.Tower, towerHealth, 100, 1);
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
            building = controller.builder.Build(TowerPrefab, save.position, Building.Types.Tower, save.health, save.level * 100, save.level);
        }
        if (save.type == Building.Types.Wall)
        {
            building = controller.builder.Build(WallPrefab, save.position, Building.Types.Wall, save.health, save.level * 100, save.level);
        }
        if (save.type == Building.Types.Gate)
        {
            building = controller.builder.Build(GatePrefab, save.position, Building.Types.Gate, save.health, save.level * 100, save.level);
        }
        DefenceSave defence = save.defence;
        if(building != null && defence != null)
        {
            GetComponent<DefenceController>().AddDefence(defence, building);
        }

    }
}
