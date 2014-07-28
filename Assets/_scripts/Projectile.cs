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

	public float Speed;

    public Attack attack;

	private void Awake () 
	{
		GameObject = gameObject;
		Transform = transform;
	}

	private void Start ()
	{
		Destroy(GameObject, LiveTime);
	}

	private void Update () 
	{
		Transform.Translate(Direction * Time.deltaTime * Speed);
	}

	public void OnTriggerEnter (Collider col)
	{
		if (attack!=null && attack.MakeAttack(col.transform))
			Destroy(GameObject);
	}
}