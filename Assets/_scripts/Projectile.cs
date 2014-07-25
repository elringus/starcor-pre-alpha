using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	[HideInInspector]
	public GameObject GameObject;
	[HideInInspector]
	public Transform Transform;

	public Vector3 Direction;
	public float LiveTime;

	public float Damage;
	public float Speed;

	public AimType AimType;
	public OwnType OwnType;

	private Attack attack;

	private void Awake () 
	{
		GameObject = gameObject;
		Transform = transform;
	}

	private void Start ()
	{
		attack = new Attack(Damage, OwnType, AimType);
		Destroy(GameObject, LiveTime);
	}

	private void Update () 
	{
		Transform.Translate(Direction * Time.deltaTime * Speed);
	}

	public void OnTriggerEnter (Collider col)
	{
		if (attack.MakeAttack(col.transform))
		{
			attack.MakeAttack(col.transform);
			Destroy(GameObject);
		}
	}
}