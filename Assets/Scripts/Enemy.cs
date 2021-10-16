using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 25;
    public int speed = 5;
    public float damage;
    public Transform target;
    private Rigidbody rb;
    public EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        //float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        Vector3 lookRotation = Quaternion.LookRotation(direction).eulerAngles;
        rb.rotation = Quaternion.Euler(Vector3.Scale(lookRotation, Vector3.up));
        var movement = direction.normalized;
        rb.MovePosition(transform.position + (speed * Time.deltaTime * movement));
    }

    public void Hurt(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            enemyController.enemies.Remove(this);
            Destroy(gameObject);
        }
    }
}
