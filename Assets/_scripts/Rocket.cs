using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rocket : MonoBehaviour
{
	public GameObject GameObject;
	public Transform Transform;

	public float Damage;
	public float Speed;

	public List<Vector3> Waypoints = new List<Vector3>();
	private Vector3 currentPoint;

	private void Awake () 
	{
		GameObject = gameObject;
		Transform = transform;

		Transform.position = Waypoints[0];
	}

	private void Update () 
	{
		if (Waypoints.Count > 0 && Transform.position == currentPoint) SetNextDestination();
	}

	private void OnTriggerEnter ()
	{
		// boom
	}

	private void SetNextDestination ()
	{
		for (int i = 0; i <= Waypoints.Count; i++)
		{
			if (Waypoints.Count == i + 1)
			{
				currentPoint = Waypoints[0];
				GameObject.MoveTo(currentPoint, Speed, 0);
				break;
			}
			else
			{
				Destroy(GameObject);
				break;
			}
		}
	}
}