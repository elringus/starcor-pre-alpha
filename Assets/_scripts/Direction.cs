using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Direction : MonoBehaviour
{
    public Transform OriginTransform;
    public GameObject LaserBeamPrototype;
    public float TargetingRadius;

    private float MaxDistance = 10f;
    private Vector3 direction;
    private Vector3 origin;
    private LineRenderer lineRenderer;

    private bool isTargeting = false;
  

	private void Awake () 
	{
        
	}

    private void Start()
    {
        StartCoroutine(Targeting());

        origin = OriginTransform.position;
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetVertexCount(2);

        //renderer.enabled = false;
        transform.localScale = new Vector3(0, 0, 0);
    }

	private void Update () 
	{
        
	}

    private IEnumerator Targeting()
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                if (!isTargeting)
                    Refresh();

                var p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                direction = new Vector3(p.x, 0, p.z);

                //Ограничения по радиусу прицеливания
                if (Vector3.Distance(origin, direction) > TargetingRadius)
                    FinishTargeting(false);
                else
                    lineRenderer.SetPosition(1, direction);
            }

            if (Input.GetMouseButtonUp(0) && isTargeting)
                FinishTargeting(true);

            yield return new WaitForSeconds(0.002f);
        }
    }


    private void FinishTargeting(bool createLaser)
    {
        isTargeting = false;
        lineRenderer.enabled = false;
        transform.localScale = new Vector3(0, 0, 0);
        if (createLaser)
            CreateLaserBeam();
    }

    private void CreateLaserBeam()
    {
        var destination = (direction - origin).normalized * MaxDistance + origin;
        ((GameObject) Instantiate(LaserBeamPrototype, origin, Quaternion.identity)).GetComponent<LaserBeam>().Instantiate(origin,destination);
    }

    private void Refresh()
    {
        isTargeting = true;
        lineRenderer.enabled = true;
        transform.localScale = new Vector3(TargetingRadius * 2, 0, TargetingRadius * 2);
        //renderer.enabled = true;
    }
}