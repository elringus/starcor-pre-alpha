using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LaserBeam : MonoBehaviour
{
    public float LifeTime;
    public float BeamWidth;

    private float currTime;
    private LineRenderer lineRenderer;

    public void Instantiate(Vector3 origin, Vector3 destination)
    {
        lineRenderer.SetPosition(0, origin);
        var hit = Hit(origin, destination);
        if (hit.transform == null)
            lineRenderer.SetPosition(1, destination);
        else
            lineRenderer.SetPosition(1, hit.transform.position);
        
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetWidth(BeamWidth, BeamWidth);
    }

    private RaycastHit Hit(Vector3 origin, Vector3 destination)
    {
        RaycastHit hit;
        if (Physics.SphereCast(new Ray(origin, destination), BeamWidth / 2, out hit))
        {
            if (hit.transform.tag == "Enemy")
                hit.transform.GetComponent<Enemy>().TakeDamage(8);
        }
        return hit;
    }

	private void Update () 
	{
        if (currTime < LifeTime)
            currTime += Time.deltaTime;
        else
            Destroy(gameObject);
	}

   

    
}