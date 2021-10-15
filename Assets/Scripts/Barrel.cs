using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject impactEffect;
    // Start is called before the first frame update
    void Start()
    {
        GameObject effectInstance = (GameObject )Instantiate(impactEffect,transform.position,transform.rotation);
        Destroy(effectInstance,5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
