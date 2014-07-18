using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Trajectory : MonoBehaviour
{
    public float StepLength;
    public GameObject RocketPrototype;

    private List<Vector3> Points { get; set; }
    private LineRenderer lineRenderer;
    private int maxVertCount = 200;
    private int currVertCount = 0;
	private bool inConstruction = false;

	private void Awake () 
	{
        Points = new List<Vector3>();
	}

    private void Start()
    {
        StartCoroutine(GenPoints());
       
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetVertexCount(maxVertCount);
    }

	private void Update () 
	{
        
	}

    private IEnumerator GenPoints()
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                if (!inConstruction)
                {
                    Refresh();
                    inConstruction = true;
                }
                var p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                if (Points.Count < maxVertCount)
                    AddVertex(new Vector3(p.x, 0, p.z));
            }

            if (Input.GetMouseButtonUp(0))
            {
                inConstruction = false;
                if (Points.Count < 2)
                    lineRenderer.enabled = false;
                else
                {
                    CreateRocket();
                    for (int i = currVertCount; i < maxVertCount; i++)
                        lineRenderer.SetPosition(i, Points.Last());
                }
            }

            yield return new WaitForSeconds(0.002f);
        }
    }

    private void AddVertex(Vector3 p)
    {
        if (Points.Count == 0 || Vector3.Distance(Points.Last(), p) >= StepLength)
        {
            Points.Add(p);
            lineRenderer.SetPosition(currVertCount, p);
            currVertCount++;
        }

        for (int i = currVertCount; i < maxVertCount; i++)
            lineRenderer.SetPosition(i, p);
    }
   
    private void CreateRocket()
    {
        var rocky = ((GameObject)Instantiate(RocketPrototype, Points[0], Quaternion.identity));
        rocky.GetComponent<Rocket>().Initialize(Points, StepLength);
    }

    private void Refresh()
    {
        Points.Clear();
        lineRenderer.enabled = true;
        currVertCount = 0;
    }
}