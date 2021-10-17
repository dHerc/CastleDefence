using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public GameObject impactEffect;
    private double lifetime = 5;

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitTarget(collision.gameObject);
        }
    }

    void HitTarget(GameObject target)
    {
        if (impactEffect)
        {
            GameObject effectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectInstance, 2f);
        }
        var enemy = target.GetComponent<Enemy>();
        if(enemy)
            enemy.Hurt(damage);
        
    }
}
