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
    //public bool UseITween;

	private Vector3[] Waypoints;
	private void Awake () 
	{
		GameObject = gameObject;
		Transform = transform;
	}

    private void Start()
    {
        float sumLength = 0;
        for (int i = 0; i < Waypoints.Length - 1; i++)
            sumLength += Vector3.Distance(Waypoints[i], Waypoints[i + 1]);
        GameObject.MoveTo(Waypoints, sumLength / Speed, 0, EaseType.linear);
    }

    public void Initialize(List<Vector3> points)
    {
        Waypoints = points.ToArray();
    }

    public void Update()
    {
       
    }

    public void OnCollisionEnter(Collision col)
    {
		if (col.collider.CompareTag("Enemy"))
		{
			Destroy(col.gameObject);
			Destroy(GameObject);
		}
		else if (col.collider.CompareTag("Obstacle"))
		{
			Destroy(GameObject);
		}
    }
}