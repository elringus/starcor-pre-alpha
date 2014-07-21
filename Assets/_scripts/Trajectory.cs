﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Trajectory : MonoBehaviour
{
    public float StepLength;
    public float DeathDistanse;
    public float InteropFactor;
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
                    for (int i = currVertCount; i < maxVertCount; i++)
                        lineRenderer.SetPosition(i, Points.Last());
                    CreateRocket();
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

    private List<Vector3> GetInteropPoints()
    {
        int iCount; 
        float iStep = StepLength/InteropFactor; //length of intrepolation step
        List<Vector3> iPoinst = new List<Vector3>();
        for (int i = 0; i < Points.Count - 1; i++)
        {
            iCount = Mathf.CeilToInt(Vector3.Distance(Points[i], Points[i + 1]) / iStep);
            for (int j = 0; j < iCount - 1; j++)
                iPoinst.Add((Points[i + 1] - Points[i]).normalized * iStep * j + Points[i]);
        }

        return iPoinst;
    }


    private void CreateRocket()
    {
        var rocky = ((GameObject)Instantiate(RocketPrototype, Points[0], Quaternion.Euler(new Vector3(90, 0, 0))));
        var v=(Points[Points.Count - 1] - Points[Points.Count - 2]).normalized;
        var lastpoint =Points[Points.Count - 1];
        for (int i = 1; i <= Mathf.CeilToInt(DeathDistanse / StepLength); i++)
            Points.Add(v * StepLength * i + lastpoint);
        rocky.GetComponent<Rocket>().Initialize(GetInteropPoints());
    }

    private void Refresh()
    {
        Points.Clear();
        lineRenderer.enabled = true;
        currVertCount = 0;
    }
}