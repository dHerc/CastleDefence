using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
	public Transform target;

	[Header("Attributes")]
	public float range = 15f;
	public float bulletSpeed = 0.1f;
	public float fireRate = 1f;
	public float damage = 5;
	private float fireCountdown=0f;

	[Header("Unity Setup Fields")]
	public string enemyTag="Enemy";

	public Transform partToRotate;

	public float turnSpeed = 10f;

	public GameObject bulletPrefab;
	public Transform firePoint;



	// Use this for initialization
	void Start () {
	InvokeRepeating("UpdateTarget",0f,0.5f);
	}

	void UpdateTarget()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;

		foreach(GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if(distanceToEnemy < shortestDistance) 
			{
				shortestDistance =distanceToEnemy;
				nearestEnemy = enemy;
			}
		}
		if(nearestEnemy!=null && shortestDistance<=range){
			target = nearestEnemy.transform;

		}
		else{
			target = null;
		}
	}

	
	// Update is called once per frame
	void Update () {
		if(target == null) return;

		Vector3 dir = target.position - transform.position;
		if(dir!=Vector3.zero)
		{
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,lookRotation,Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f,rotation.y,0f);
		}
		

		if(fireCountdown<=0)
		{
			Shoot();
			fireCountdown=1f/fireRate;
		}

		fireCountdown-=Time.deltaTime;
	}

	void Shoot()
	{
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		var direction = target.transform.position - firePoint.position;
		direction /= Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));
		bullet.GetComponent<Bullet>().damage = damage;
		bullet.GetComponent<Rigidbody>().velocity = direction*bulletSpeed;
		bullet.gameObject.transform.rotation = Quaternion.LookRotation(direction);
	}
}
