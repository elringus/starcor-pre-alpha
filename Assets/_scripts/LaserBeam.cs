using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LaserBeam : MonoBehaviour
{
    public float LifeTime;
    public float BeamWidth;
    public float Damage;

    public AimType AimType;
    private OwnType OwnType = OwnType.Terran;

    private float currTime;
    private LineRenderer lineRenderer;

    private Attack attack;
    public void Instantiate(Vector3 origin, Vector3 destination)
    {
        lineRenderer.SetPosition(0, origin);
        var hitPoint = GetHitPoint(origin, destination);
        if (hitPoint == null)
            lineRenderer.SetPosition(1, destination);
        else
            lineRenderer.SetPosition(1, hitPoint.Value);
        
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetWidth(BeamWidth, BeamWidth);
        attack = new Attack(Damage, OwnType, AimType);
    }

    private Vector3? GetHitPoint(Vector3 origin, Vector3 destination)
    {
        RaycastHit hit;
        if (Physics.SphereCast(new Ray(origin, destination - origin), BeamWidth / 2, out hit))
        {
            if (attack.MakeAttack(hit.transform))
                return hit.point;
        }
        return null;
    }

	private void Update () 
	{
        if (currTime < LifeTime)
            currTime += Time.deltaTime;
        else
            Destroy(gameObject);
	}

   

    
}