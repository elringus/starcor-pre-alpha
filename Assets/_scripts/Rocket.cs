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
    private float stepTime;
    private float currTime = 0;
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
        Transform.LookAt(Waypoints[1]);
        GameObject.MoveTo(Waypoints, sumLength / Speed, 0, EaseType.linear);
        stepTime = sumLength / (Speed * Waypoints.Length);
    }

    public void Initialize(List<Vector3> points)
    {
        Waypoints = points.ToArray();
    }

    public void Update()
    {
        currTime += Time.deltaTime;
        int pointIndex=Mathf.CeilToInt(currTime/stepTime);
        if (pointIndex < Waypoints.Length)
            GameObject.LookTo(Waypoints[pointIndex], 2f, 0);
    }

    public void OnCollisionEnter(Collision col)
    {
        Destroy(col.gameObject);
        Destroy(GameObject);
    }
}