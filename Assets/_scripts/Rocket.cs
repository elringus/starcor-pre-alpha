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
    public float AngularSpeed;

	public float ThrowPower;
	public float ExplosionRadius;

    private int currIndex = 0;
    private int lastIndex = -1;

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

        currIndex=Mathf.CeilToInt(currTime/stepTime);
        if (currIndex < Waypoints.Length)
        {
            if (currIndex != lastIndex)
            {
                float angularDistance = Math.Abs(Transform.rotation.eulerAngles.y - eMath.Angle2Dplus(Transform.position, Waypoints[currIndex]) * Mathf.Rad2Deg);
                if (angularDistance > 180) angularDistance = 360 - angularDistance;
                GameObject.LookTo(Waypoints[currIndex], angularDistance / AngularSpeed, 0);
                lastIndex = currIndex;
            }
        }
        else
            Destroy(GameObject);
        
    }

    public void OnTriggerEnter(Collider col)
    {
        bool needExplode = true;
        switch (col.collider.tag)
        {
            case "Enemy":
                Destroy(col.gameObject);
                break;
            //case "Rocket":
            //    Destroy(col.gameObject);
            //    break;
            case "Obstacle":
                break;
            case "Static":
                break;
            default:
                needExplode = false;
                break;
        }

        if (needExplode)
            Explode();
    }

    private void Explode()
    {
        var colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach (var hit in colliders)
            if (hit && hit.rigidbody)
                hit.rigidbody.AddExplosionForce(ThrowPower, transform.position, ExplosionRadius, 0.1f, ForceMode.Impulse);
        Destroy(GameObject);
    }
}