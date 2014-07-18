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
	private float stepLength;
    private int accValue = 1000;
    private Vector3 deathPoint;
	private void Awake () 
	{
		GameObject = gameObject;
		Transform = transform;
	}

	private void Start ()
	{
        float sumLength = 0;
        for(int i=0; i<Waypoints.Length-1; i++)
            sumLength+=Vector3.Distance(Waypoints[i], Waypoints[i + 1]);
        GameObject.MoveTo(Waypoints, sumLength / Speed, 0, EaseType.linear);
	}

    public void Initialize(List<Vector3> points, float stepLength)
    {
        Waypoints = points.ToArray();
		this.stepLength = stepLength;
    }

    public void Update()
    {
        if (Mathf.Round(Vector3.Distance(Waypoints.Last(), Transform.position) * accValue) / accValue == 0)
        {
            Vector3 lp = Waypoints[Waypoints.Length - 1];
            Vector3 prelp = Waypoints[Waypoints.Length - 2];
            deathPoint = (lp - prelp).normalized * 100 + lp;
            GameObject.MoveTo(deathPoint, Vector3.Distance(lp, deathPoint) / Speed, 0, EaseType.linear);
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        Destroy(col.gameObject);
        Destroy(GameObject);
    }
}