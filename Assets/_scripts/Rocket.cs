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
	private Vector3 currentPoint;
    private Vector3 nextpoint;
    private float fracJourney;
    private int currIX = 0;

    private float currTime;
    private float currLine;

    private int accValue = 1000;
	private void Awake () 
	{
		GameObject = gameObject;
		Transform = transform;
	}

    private void Start()
    {
    }
    private void Update()
    {
        if (Mathf.Round(Vector3.Distance(Waypoints[currIX], Transform.position) * accValue) / accValue == 0)
        {
            if (currIX + 1 < Waypoints.Length)
            {
                currTime = Time.time;
                currIX++;
                currLine = Vector3.Distance(Waypoints[currIX - 1], Waypoints[currIX]);
                if (UseITween)
                    GameObject.MoveTo(Waypoints[currIX], currLine / Speed, 0, EaseType.linear);
            }
            else
            {
                Debug.Log("kill");
                Destroy(GameObject);
            }
        }
        else if(!UseITween)
        {
            float dt = Time.time - currTime;
            transform.position = Vector3.Lerp(Waypoints[currIX-1], Waypoints[currIX], dt / (currLine / Speed));
        }
    }

	private void OnTriggerEnter ()
	{
	}

	private void SetNextDestination ()
	{
		
	}

    public void SetWaypoints(List<Vector3> points)
    {
        Waypoints = points.ToArray();
    }

    public void OnCollisionEnter(Collision col)
    {
        Destroy(col.gameObject);
        Destroy(GameObject);
        Debug.Log("You are big killer!");
    }
}