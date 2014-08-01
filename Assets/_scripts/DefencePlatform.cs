using UnityEngine;
using System.Collections;

public class DefencePlatform : MonoBehaviour
{
	public Transform Earth;
	public Transform Transform;

	private void Awake () 
	{
		Transform = transform;
	}

	private void Update () 
	{
		Transform.RotateAround(Earth.position, Vector3.up, Time.deltaTime * 3);
	}
}