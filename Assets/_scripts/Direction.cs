using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Direction : MonoBehaviour
{
    public Transform OriginTransform;
    public GameObject LaserBeamPrototype;

    private float MaxDistance = 10f;
    private Vector3 direction;
    private Vector3 origin;
    private LineRenderer lineRenderer;

    private bool isSighting = false;
  

	private void Awake () 
	{
	}

    private void Start()
    {
        StartCoroutine(Sighting());

        origin = OriginTransform.position;
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetVertexCount(2);
    }

	private void Update () 
	{
        
	}

    private IEnumerator Sighting()
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                if (!isSighting)
                    Refresh();

                var p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                direction = new Vector3(p.x, 0, p.z);
                lineRenderer.SetPosition(1, direction);
            }

            if (Input.GetMouseButtonUp(0))
            {
                isSighting = false;
                lineRenderer.enabled = false;
                CreateLaserBeam();
            }

            yield return new WaitForSeconds(0.002f);
        }
    }

    private void CreateLaserBeam()
    {
        var destination = (direction - origin).normalized * MaxDistance + origin;
        ((GameObject) Instantiate(LaserBeamPrototype, origin, Quaternion.identity)).GetComponent<LaserBeam>().Instantiate(origin,destination);
    }

    private void Refresh()
    {
        isSighting = true;
        lineRenderer.enabled = true;
    }
}