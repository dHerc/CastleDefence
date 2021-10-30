using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float speed;
    public float damage;
    public Vector3Int loot;
    public Transform target;
    public enum Enemies {Warrior, Ram, Bomber};
    public Enemies type;
    private Rigidbody rb;
    public EnemyController enemyController;
    private Animator animator;
    private float damageScale;
    private Renderer[] enemyRenderers;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        enemyRenderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in enemyRenderers)
        {
            renderer.material = new Material(renderer.sharedMaterial);
            renderer.sharedMaterial.SetFloat("_PermamentDamageScale", 0);
        }

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
        if (damageScale > 0f)
        {
            foreach (var renderer in enemyRenderers)
            {
                renderer.sharedMaterial.SetFloat("_TemporaryDamageScale", damageScale);
                damageScale -= 0.05f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var building = collision.gameObject;
        if (building.CompareTag("Building"))
        {
            if(animator)
                animator.SetTrigger("Attack");
            building.GetComponent<Building>().Damage(damage);
            Vector3 direction = Vector3.Scale(transform.position - building.transform.position, new Vector3(1, 0, 1));
            var movement = direction.normalized;
            rb.velocity = movement * 2f;
        }
        if (building.CompareTag("Castle"))
        {
            if(animator)
                animator.SetTrigger("Attack");
            building.GetComponentInParent<GameController>().Damage(damage);
            Vector3 direction = Vector3.Scale(transform.position - building.transform.position, new Vector3(1, 0, 1));
            var movement = direction.normalized;
            rb.velocity = movement * 2f;
        }
        if (type == Enemies.Bomber)
            Hurt(health + 1);
    }

    public void Hurt(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            enemyController.AddLoot(loot);
            enemyController.enemies.Remove(this);
            Destroy(gameObject);
        }
        else
        {
            foreach (var renderer in enemyRenderers)
            {
                if (renderer)
                {
                    renderer.sharedMaterial.SetFloat("_PermamentDamageScale", 1f - (health / 100));
                    damageScale = 1f;
                }
            }
        }
    }
}
