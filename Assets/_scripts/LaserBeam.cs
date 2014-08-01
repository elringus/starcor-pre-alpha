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
    protected LineRenderer lineRenderer;
    protected Transform Transform;
    protected GameObject GameObject;
    protected Attack attack;
    public virtual void Instantiate(Vector3 origin, Vector3 destination)
    {
        lineRenderer.SetPosition(0, origin);
        var hitTarget = GetTarget(origin, destination);
        if (hitTarget == null)
            lineRenderer.SetPosition(1, destination);
        else
            lineRenderer.SetPosition(1, hitTarget.Value.point);
        
    }

    protected virtual void Awake()
    {
        Transform = transform;
        GameObject = gameObject;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetWidth(BeamWidth, BeamWidth);
        attack = new Attack(Damage, OwnType, AimType);
    }

    protected virtual RaycastHit? GetTarget(Vector3 origin, Vector3 destination)
    {
        RaycastHit hit;
        if (Physics.SphereCast(new Ray(origin, destination - origin), BeamWidth / 2, out hit))
        {
            if (attack.MakeAttack(hit.transform))
                return hit;
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