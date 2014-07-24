using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LaserBeam : MonoBehaviour
{
    public float LifeTime;
    public float BeamWidth;
    public float Damage;

    private float currTime;
    private LineRenderer lineRenderer;

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
    }

    private Vector3? GetHitPoint(Vector3 origin, Vector3 destination)
    {
        RaycastHit hit;
        if (Physics.SphereCast(new Ray(origin, destination - origin), BeamWidth / 2, out hit))
        {
            if (hit.transform.GetComponent(typeof(IAttackable)))
                (hit.transform.GetComponent(typeof(IAttackable)) as IAttackable).RecieveAtatck(new Attack(Damage, FOF.Friend));

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