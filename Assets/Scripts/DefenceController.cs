using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceController : MonoBehaviour
{
    [SerializeField] private GameObject archer;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject barrel;

    private Color color;
    private GameObject building;
    private GameController controller;
    private bool buildingWall = false;

    void Start()
    {
        controller = gameObject.GetComponent<GameController>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1<<9) && !controller.isAttack)
        {
            if (Input.GetMouseButtonDown(0) )
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    if (building)
                    {
                        if (!building.Equals(hit.collider.gameObject))
                        {
                            building.GetComponentInChildren<Renderer>().material.color = color;
                            
                            if (buildingWall && building.GetComponent<Building>().type == Building.Types.Tower && hit.collider.gameObject.GetComponent<Building>().type == Building.Types.Tower)
                            {
                                var nextBuilding = hit.collider.gameObject;
                                if(building.transform.position.x == nextBuilding.transform.position.x)
                                {
                                    BuildWall(building.transform.position, nextBuilding.transform.position, false);
                                }
                                if (building.transform.position.z == nextBuilding.transform.position.z)
                                {
                                    BuildWall(building.transform.position, nextBuilding.transform.position, true);
                                }
                                building.GetComponent<Building>().wallIds.Add(controller.maxWallId-1);
                                nextBuilding.GetComponent<Building>().wallIds.Add(controller.maxWallId-1);
                                buildingWall = false;
                                building = null;
                            }
                            else
                            {
                                building = hit.collider.gameObject;
                                building.GetComponentInChildren<Renderer>().material.color = Color.green;
                            }
                        }
                        else
                        {
                            building.GetComponentInChildren<Renderer>().material.color = color;
                            buildingWall = false;
                            building = null;
                        }
                    }
                    else
                    {
                        color = hit.collider.gameObject.GetComponentInChildren<Renderer>().material.color;
                        building = hit.collider.gameObject;
                        building.GetComponentInChildren<Renderer>().material.color = Color.green;
                    }
                }
            }
            if (building)
            {
                if (building.GetComponent<Building>().inside == null)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        controller.builder.AddDefence(turret, building, Defence.Defences.Turret, 1);
                    }
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        controller.builder.AddDefence(archer, building, Defence.Defences.Archer, 1);
                    }
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        controller.builder.AddDefence(barrel, building, Defence.Defences.Barrel, 1);
                    }
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        buildingWall = true;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        building.GetComponent<Building>().inside.GetComponent<Defence>().Trigger();
                    }
                }
            }
        }
        if(controller.isAttack)
        {
            foreach(Building building in controller.buildings)
            {
                building.GetComponentInChildren<Renderer>().material.color=color;
            }
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
        test.y+=2;
        if (Physics.SphereCast(test, size/3, new Vector3(0,-1,0), out hit, 3f, 1 << 9))
        {
            return;
        }
        if(alongX)
        {
            if(start.x>end.x)
            {
                var tmp = start;
                start = end;
                end = tmp;
            }
            for(float i = start.x + size; i < end.x; i++)
            {
                float height = start.y + (end.y - start.y) * ((i - start.x) / (end.x - i)) / (end.x - start.x);
                buildingController.BuildWall(new Vector3(i, height, start.z), new Vector3(0,0,0));
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
