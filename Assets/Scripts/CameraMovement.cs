using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float zoomSpeed;
    public float orthographicSizeMin;
    public float orthographicSizeMax;
    public float fovMin;
    public float fovMax;
    private Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
    }
    void Update()
    {
        if (camera.orthographic)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                camera.orthographicSize += zoomSpeed;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                camera.orthographicSize -= zoomSpeed;
            }
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, orthographicSizeMin, orthographicSizeMax);
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                camera.fieldOfView += zoomSpeed;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                camera.fieldOfView -= zoomSpeed;
            }
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, fovMin, fovMax);
        }
    }
}
