using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private GameObject WallPrefab;
    [SerializeField] private GameObject TowerPrefab;
    [SerializeField] private GameObject GatePrefab;
    public float Size;

    private Vector3 target;
    private Vector3 position;

    void Start()
    {

    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            target = hit.point;
            position.x = Mathf.Round(target.x / Size) * Size;
            position.y = 0.5f;
            position.z = Mathf.Round(target.z / Size) * Size;
            if (Input.GetMouseButtonDown(1))
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    Destroy(hit.collider.gameObject);
                }
                else
                {
                    var building = Instantiate(TowerPrefab, position, Quaternion.identity);
                    building.layer = 9;
                }
                //wall.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                //wall.transform.position = new Vector3(wall.transform.position.x, 5, wall.transform.position.z);
            }
        }
    }
}
