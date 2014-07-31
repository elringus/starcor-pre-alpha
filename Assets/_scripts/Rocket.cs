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

	public GameObject[] VFX;

	public float Damage;
	public float Speed;
    public float AngularSpeed;

	public float ThrowPower;
	public float ExplosionRadius;

    public AimType AimType;
    [HideInInspector]
    public Tower BaseTower;

    private int currIndex = 0;
    private int lastIndex = -1;
    private OwnType OwnType = OwnType.Terran;
	private Vector3[] Waypoints;
    private float stepTime;
    private float currTime = 0;
    private Attack attack;
	private void Awake () 
	{
        attack = new Attack(Damage, OwnType, AimType);
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

    public void Initialize(List<Vector3> points, Tower baseTower)
    {
        Waypoints = points.ToArray();
        BaseTower = baseTower;
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
                GameObject.LookUpdate(Waypoints[currIndex], angularDistance / AngularSpeed);
                lastIndex = currIndex;
            }
        }
        else Destroy(GameObject);
        
    }

    public void OnTriggerEnter(Collider col)
    {
		if (attack.CanAttack(col.transform) != null)
		{
			Instantiate(VFX[UnityEngine.Random.Range(0, VFX.Length)], Transform.position + new Vector3(0, 1, 0), Quaternion.identity);
			if (gameObject.name == "rocket(Clone)") CameraController.I.Shake(.8f, .5f);
			else CameraController.I.Shake(.05f, 0.1f);
			Explode(attack, col);
		}
    }

    private void Explode(Attack attack, Collider centralTarget)
    {
        var colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach (var hit in colliders)
            if (hit != centralTarget)
                attack.MakeAttack(hit.transform, CalcThrowPower(hit.transform.position), CalcSplashDamage(hit.transform.position));
            else
                attack.MakeAttack(hit.transform, CalcThrowPower(hit.transform.position), Damage);

        Destroy(GameObject);
    }

    private Vector3 CalcThrowPower(Vector3 target)
    {
        Vector3 radius = target - transform.position;
        if (radius.magnitude < ExplosionRadius)
            return radius.normalized * (1 - Mathf.Pow((radius.magnitude / ExplosionRadius), 2)) * ThrowPower;
        else
            return Vector3.zero;
    }

    private float CalcSplashDamage(Vector3 target)
    {
        Vector3 radius = target - transform.position;
        if (radius.magnitude < ExplosionRadius)
            return (1 - Mathf.Pow((radius.magnitude / ExplosionRadius), 1)) * Damage;
        else
            return 0;
    }
}