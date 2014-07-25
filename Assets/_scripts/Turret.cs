using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour
{
	[HideInInspector]
	private Transform Transform;
	[HideInInspector]
	public SphereCollider Collider;

	public GameObject Projectile;

	public float CD;
	public float Range;

	public Transform CurrentTarget;

	private float timer;

	private void Awake () 
	{
		Transform = transform;
		Collider = collider as SphereCollider;
		Collider.radius = Range;
	}

	private void Update () 
	{
		if (CurrentTarget && Vector3.Distance(Transform.position, CurrentTarget.position) > Range)
			CurrentTarget = null;

		timer = timer > 0 ? timer - Time.deltaTime : 0;

		if (CurrentTarget && timer == 0) Shoot();
	}

	private void OnTriggerStay (Collider colli)
	{
		if (CurrentTarget) return;

		if (colli.GetComponent(typeof(IAttackable)) && ((IAttackable)colli.GetComponent(typeof(IAttackable))).GetOwnType == OwnType.Allien)
			CurrentTarget = colli.transform;
	}

	private void Shoot ()
	{
		timer = CD;

		var projectile = (Instantiate(Projectile, Transform.position, Quaternion.identity) as GameObject).GetComponent<Projectile>();
		projectile.AimType = AimType.Foe;
		projectile.OwnType = OwnType.Terran;
		projectile.Direction = (CurrentTarget.position - Transform.position).normalized;
	}
}