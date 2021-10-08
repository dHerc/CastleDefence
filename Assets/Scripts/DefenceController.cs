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

    void Start()
    {

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
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        Instantiate(army, building.transform);
                    }
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        Instantiate(archer, building.transform);
                    }
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        Instantiate(barrel, building.transform);
                    }
            }
        }
    }
}
