using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceController : MonoBehaviour
{
    [SerializeField] private GameObject archer;
    [SerializeField] private GameObject army;
    [SerializeField] private GameObject barrel;

    private Color color;
    private GameObject building;
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
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    if (building)
                    {
                        if (!building.Equals(hit.collider.gameObject))
                        {
                            building.GetComponentInChildren<Renderer>().material.color = color;
                            building = hit.collider.gameObject;
                            building.GetComponentInChildren<Renderer>().material.color = Color.green;
                        }
                        else
                        {
                            building.GetComponentInChildren<Renderer>().material.color = color;
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
                        controller.builder.AddDefence(army, building, Defence.Defences.Army, 1);
                    }
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        controller.builder.AddDefence(archer, building, Defence.Defences.Archer, 1);
                    }
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        controller.builder.AddDefence(barrel, building, Defence.Defences.Barrel, 1);
                    }
                }
            }
        }
    }
    public void AddDefence(DefenceSave save, GameObject parent)
    {
        if (save.type == Defence.Defences.Army)
        {
            controller.builder.AddDefence(army, parent, save.type, save.level);
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
}
