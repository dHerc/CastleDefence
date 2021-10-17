using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject impactEffect;
    public GameObject tarPrefab;
    public float damage;
    private bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Trigger()
    {
        triggered = true;
        GameObject effectInstance = (GameObject )Instantiate(impactEffect,transform.position,transform.rotation);
        for (float i = 0; i < 1; i += 0.05f)
        {
            InvokeRepeating(nameof(CreateTar), 0f, 0.05f);
        }
        var corutine = End(effectInstance);
        StartCoroutine(corutine);
    }

    private void CreateTar()
    {
        GameObject bullet = Instantiate(tarPrefab, transform.position, transform.rotation);
        var direction = new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f));
        bullet.GetComponent<Bullet>().damage = damage;
        bullet.GetComponent<Rigidbody>().velocity = direction * 2;
    }

    private IEnumerator End(GameObject efffect)
    {
        yield return new WaitForSeconds(3);
        Destroy(efffect);
        GetComponentInParent<Defence>().Remove();
        Destroy(gameObject);
    }
}
