using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
	public Transform Transform;

	public float XSpeed;
	public float YSpeed;
	public float ZSpeed;

	private void Awake () 
	{
		Transform = transform;
	}

	private void Update () 
	{
		Transform.Rotate(new Vector3(XSpeed * Time.deltaTime, YSpeed * Time.deltaTime, ZSpeed * Time.deltaTime));
	}
}