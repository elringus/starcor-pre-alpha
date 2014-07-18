using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Rocket : MonoBehaviour
{
    [HideInInspector]
	public GameObject GameObject;
    [HideInInspector]
	public Transform Transform;

	public float Damage;
	public float Speed;
    public bool UseITween;

	private Vector3[] Waypoints;
	private float stepLength;

	private void Awake () 
	{
		GameObject = gameObject;
		Transform = transform;
	}

	private void Start ()
	{
		GameObject.MoveTo(Waypoints, Waypoints.Length * stepLength / Speed, 0, EaseType.linear);
	}

    public void Initialize(List<Vector3> points, float stepLength)
    {
        Waypoints = points.ToArray();
		this.stepLength = stepLength;
    }

    public void OnCollisionEnter(Collision col)
    {
        Destroy(col.gameObject);
        Destroy(GameObject);
    }
}