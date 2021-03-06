using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceController : MonoBehaviour
{
    [SerializeField] private GameObject archer;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject barrel;

    private Color color = new Color(106f/255f,106f/255f,106f/255f);
    public GameObject building;
    private GameController controller;
    private UIController UIcontroller;
    private bool buildingWall = false;

    void Start()
    {
        controller = gameObject.GetComponent<GameController>();
        UIcontroller = gameObject.GetComponent<UIController>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Defences")) && controller.isAttack && Input.GetMouseButtonDown(0))
        {
            var barrel = hit.collider.gameObject.GetComponentInChildren<Barrel>();
            if (barrel)
                barrel.Trigger();
        }
        else if (Physics.Raycast(ray, out hit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Defences")) && !controller.isAttack && Input.GetMouseButtonDown(0))
        {
            var defence = hit.collider.gameObject.GetComponent<Defence>();
            if (defence)
                defence.Upgrade();
        }
        else if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask.GetMask("Buildings","Outline")) && !controller.isAttack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (building)
                {
                    if (!building.Equals(hit.collider.gameObject))
                    {
                        building.GetComponent<Building>().Deselect();

                        if (buildingWall && building.GetComponent<Building>().type == Building.Types.Tower && hit.collider.gameObject.GetComponent<Building>().type == Building.Types.Tower)
                        {
                            var nextBuilding = hit.collider.gameObject;
                            if (GetComponent<BuildingController>().Pay(
                                (int)(Mathf.Abs(building.transform.position.x - nextBuilding.transform.position.x) +
                                Mathf.Abs(building.transform.position.z - nextBuilding.transform.position.z)-1)))
                            {
                                if (building.transform.position.x == nextBuilding.transform.position.x)
                                {
                                    BuildWall(building.transform.position, nextBuilding.transform.position, false);
                                }
                                if (building.transform.position.z == nextBuilding.transform.position.z)
                                {
                                    BuildWall(building.transform.position, nextBuilding.transform.position, true);
                                }
                                building.GetComponent<Building>().wallIds.Add(controller.maxWallId - 1);
                                nextBuilding.GetComponent<Building>().wallIds.Add(controller.maxWallId - 1);
                            }
                            buildingWall = false;
                            building = null;
                        }
                        else
                        {
                            building = hit.collider.gameObject;
                            building.GetComponent<Building>().Select();
                        }
                    }
                    else
                    {
                        building.GetComponent<Building>().Deselect();

                        buildingWall = false;
                        building = null;
                    }
                }
                else
                {
                    building = hit.collider.gameObject;

                    if(building.GetComponent<Building>().Upgrade())
                    {
                        building = null;
                    }
                    else
                    {
                        building.GetComponent<Building>().Select();
                    }
                }
            }
            if (building)
            {
                if (building.GetComponent<Building>().inside == null)
                {
                    //UIcontroller.showWeapon();
                    if (UIcontroller.turretClicked)
                    {
                        if(Pay(Defence.Defences.Turret,1))
                            controller.builder.AddDefence(turret, building, Defence.Defences.Turret, 1);
                    }
                    if (UIcontroller.archerClicked)
                    {
                        if (Pay(Defence.Defences.Archer, 1))
                            controller.builder.AddDefence(archer, building, Defence.Defences.Archer, 1);
                    }
                    if (UIcontroller.barrelClicked)
                    {
                        if (Pay(Defence.Defences.Barrel, 1))
                            controller.builder.AddDefence(barrel, building, Defence.Defences.Barrel, 1);
                    }
                    if (UIcontroller.wallClicked)
                    {
                        buildingWall = true;
                    }
                }
            }
        }
       
    }
    public void DeselectTowers()
    {
        this.building = null;
        foreach (Building building in controller.buildings)
        {
            building.GetComponent<Building>().Deselect();
        }
    }
    public void AddDefence(DefenceSave save, GameObject parent)
    {
        if (save.type == Defence.Defences.Turret)
        {
            controller.builder.AddDefence(turret, parent, save.type, save.level);
        }
        if (save.type == Defence.Defences.Archer)
        {
            controller.builder.AddDefence(archer, parent, save.type, save.level);
        }
        if (save.type == Defence.Defences.Barrel)
        {
            controller.builder.AddDefence(barrel, parent, save.type, save.level);
        }
    }

    public bool Pay(Defence.Defences type, int level)
    {
        var payment = new Vector3Int(0, 0, 0);
        if(type == Defence.Defences.Archer)
        {
            payment.x += (int)Mathf.Pow(level, 1.5f) * 20;
        }
        if (type == Defence.Defences.Turret)
        {
            payment.x += (int)Mathf.Pow(level, 1.5f) * 10;
            payment.z += (int)Mathf.Pow(level, 1.5f) * 10;
        }
        if (type == Defence.Defences.Barrel)
        {
            payment.x += (int)Mathf.Pow(level, 1.5f) * 10;
            payment.y += (int)Mathf.Pow(level, 1.5f) * 10;
        }
        return controller.Pay(payment);
    }
    public void BuildWall(Vector3 start, Vector3 end, bool alongX)
    {
        var buildingController = GetComponent<BuildingController>();
        var size = buildingController.Size;
        RaycastHit hit;
        var test = start;
        if (alongX)
            if (start.x > end.x)
                test.x -= size;
            else
                test.x += size;
        else
            if (start.z > end.z)
            test.z -= size;
        else
            test.z += size;
        test.y += 2;
        if (Physics.SphereCast(test, size / 3, new Vector3(0, -1, 0), out hit, 3f, 1 << 9))
        {
            return;
        }
        if (alongX)
        {
            if (start.x > end.x)
            {
                var tmp = start;
                start = end;
                end = tmp;
            }
            for (float i = start.x + size; i < end.x; i++)
            {
                float height = start.y + (end.y - start.y) * ((i - start.x) / (end.x - i)) / (end.x - start.x);
                buildingController.BuildWall(new Vector3(i, height, start.z), new Vector3(0, 0, 0));
            }
        }
        else
        {
            if (start.z > end.z)
            {
                var tmp = start;
                start = end;
                end = tmp;
            }
            for (float i = start.z + size; i < end.z; i++)
            {
                float height = start.y + (end.y - start.y) * ((i - start.z) / (end.z - i)) / (end.z - start.z);
                buildingController.BuildWall(new Vector3(start.x, height, i), new Vector3(0, 90, 0));
            }
        }
        controller.maxWallId++;
    }
}