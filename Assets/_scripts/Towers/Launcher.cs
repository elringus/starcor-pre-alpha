using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Launcher : Tower
{
    #region Definition
    public float StepLength;
    public float DeathDistanse;
    public float InteropFactor;
    public float MinRadius;

    protected List<Vector3> Points = new List<Vector3>();
    protected LineRenderer lineRenderer;
    protected int maxVertCount = 200;
    protected int currVertCount = 0;

    protected Vector3 StartPoint;
    #endregion
    #region Base

    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetVertexCount(maxVertCount);
        StartPoint = Transform.position;
        IntialCorrect();
    }
    protected override void StartTargeting()
    {
        Points.Clear();
        lineRenderer.enabled = true;
        InProcess = true;
        currVertCount = 0;
    }
    protected override void OnTargeting()
    {
        var p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        if (Points.Count < maxVertCount)
            AddVertex(new Vector3(p.x, 0, p.z));
    }
    protected override void FinishTargeting()
    {
        if (Points.Count < 2)
            lineRenderer.enabled = false;
        else
        {
            for (int i = currVertCount; i < maxVertCount; i++)
                lineRenderer.SetPosition(i, Points.Last());
            Produce();
        }

        InProcess = false;
    }

    protected override void CancelTargeting()
    {
        InProcess = false;
        lineRenderer.enabled = false;
    }
    protected override void Produce()
    {
        base.Produce();
        AddDeathPath();
        Points.Insert(0, StartPoint);
        var rocky = ((GameObject)Instantiate(Prototype, Points[0], Quaternion.identity));
        rocky.GetComponent<Rocket>().Initialize(GetInteropPoints(Points));
    }
    
    #endregion
    #region Methods
    protected List<Vector3> GetInteropPoints(List<Vector3> points)
    {
        int iCount;
        float iStep = StepLength / InteropFactor; //length of intrepolation step
        List<Vector3> iPoinst = new List<Vector3>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            iCount = Mathf.CeilToInt(Vector3.Distance(points[i], points[i + 1]) / iStep);
            for (int j = 0; j < iCount - 1; j++)
                iPoinst.Add((points[i + 1] - points[i]).normalized * iStep * j + points[i]);
        }

        return iPoinst;
    }
    protected void AddVertex(Vector3 p)
    {
        if (Points.Count == 0 || Vector3.Distance(Points.Last(), p) >= StepLength)
        {
            if (Correct(p))
            {
                Points.Add(p);
                lineRenderer.SetPosition(currVertCount, p);
                currVertCount++;
            }
        }

        for (int i = currVertCount; i < maxVertCount; i++)
            lineRenderer.SetPosition(i, p);
    }
    protected void AddDeathPath()
    {
        var v = (Points[Points.Count - 1] - Points[Points.Count - 2]).normalized;
        var lastpoint = Points[Points.Count - 1];
        for (int i = 1; i <= Mathf.CeilToInt(DeathDistanse / StepLength); i++)
            Points.Add(v * StepLength * i + lastpoint);
    }

    private int stepCount;
    private float stepAngle;
    private void IntialCorrect()
    {
        stepCount = Mathf.CeilToInt(Mathf.PI / (Mathf.Asin(StepLength / 2 * MinRadius)) / 2);
        stepAngle = Mathf.PI / (2 * stepCount);
    }

    protected bool Correct(Vector3 p)
    {
        if (Points.Count < 2)
            return true;
        int checkVert = Points.Count - stepCount;
        int n=2;
        if (checkVert < 0)
            checkVert = 0;
        for (int i = Points.Count - 2; i >= checkVert; i--)
        {
            if (Vector3.Distance(p, Points[i]) < 2 * MinRadius * Mathf.Sin(stepAngle * n))
                return false;
            n++;
        }

        return true;
    }

    #endregion

}