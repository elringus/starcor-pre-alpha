using UnityEngine;
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

    private List<Vector3> GetInteropPoints(List<Vector3> points )
    {
        int iCount; 
        float iStep = StepLength/InteropFactor; //length of intrepolation step
        List<Vector3> iPoinst = new List<Vector3>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            iCount = Mathf.CeilToInt(Vector3.Distance(points[i], points[i + 1]) / iStep);
            for (int j = 0; j < iCount - 1; j++)
                iPoinst.Add((points[i + 1] - points[i]).normalized * iStep * j + points[i]);
        }

        return iPoinst;
    }


    private void CreateRocket()
    {
        var v=(Points[Points.Count - 1] - Points[Points.Count - 2]).normalized;
        var lastpoint =Points[Points.Count - 1];
        for (int i = 1; i <= Mathf.CeilToInt(DeathDistanse / StepLength); i++)
            Points.Add(v * StepLength * i + lastpoint);
        StartVollyePoint = (Points[0] - Points[1]).normalized * 1+Points[0];
        StartCoroutine(GenVolleys());
        //var rocky = ((GameObject)Instantiate(RocketPrototype, Points[0], Quaternion.identity));
        //rocky.GetComponent<Rocket>().Initialize(GetInteropPoints(Points));
    }

    private int[] OrderVolley = { 1, 2, 3 };
    private float OffsetValue = 0.25f;
    private Vector3 StartVollyePoint;

    private IEnumerator GenVolleys()
    {
        for (int i = 0; i < OrderVolley.Length; i++ )
        {
            MakeVolley(OrderVolley[i]);
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void MakeVolley(int count)
    {
        float correction = 0;

        if (count % 2 == 0)
            correction = OffsetValue / 2;
        else
            count--;


        for (int i = -count / 2; i <= count / 2; i++)
        {
            if (correction == 0 || i != 0)
            {
                var points = OffsetPoints(Points, OffsetValue * i - Mathf.Sign(i) * correction);
                points.Insert(0, StartVollyePoint);
                points = GetInteropPoints(points);
                var rocky = ((GameObject)Instantiate(RocketPrototype, points[0], Quaternion.identity));
                rocky.GetComponent<Rocket>().Initialize(points);
            }
        }
    }

    private List<Vector3> OffsetPoints(List<Vector3> basePoints, float offset)
    {
        List<Vector3> offsetPoints = new List<Vector3>();
        float angle = (eMath.Angle2Dplus(basePoints[0], basePoints[1]) + Mathf.PI / 2);
        Vector3 v = new Vector3(offset * Mathf.Sin(angle), 0, offset * Mathf.Cos(angle));
        for (int i = 0; i < basePoints.Count; i++)
            offsetPoints.Add(basePoints[i] + v);

        return offsetPoints;
        
    }
    private void Refresh()
    {
        Points.Clear();
        lineRenderer.enabled = true;
        currVertCount = 0;
    }
}