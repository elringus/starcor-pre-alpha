using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour
{
	[HideInInspector]
	public Transform Transform;

	public Transform Earth;

	private void Awake () 
	{
		Transform = transform;
	}

	private void Update () 
	{
		Transform.RotateAround(Earth.position, Vector3.up, Time.deltaTime);
		Transform.Rotate(new Vector3(0, 2 * Time.deltaTime, 0));
	}
}