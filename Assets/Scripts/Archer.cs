using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Turret
{
    private Animator animator;

    private  void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
		if (target == null) return;

		Vector3 dir = target.position - transform.position;
		if (dir != Vector3.zero)
		{
			Quaternion lookRotation = Quaternion.LookRotation(dir);
			Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
			partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
		}

		if (fireCountdown <= 0)
		{
			Tension();
			fireCountdown = 1f / fireRate;
		}

		fireCountdown -= Time.deltaTime;
	}

	private void Tension()
    {
		animator.SetTrigger("Shoot");
	}

	protected void Shoot()
	{
		if(target)
        {
			base.Shoot();
        }
	}
}
