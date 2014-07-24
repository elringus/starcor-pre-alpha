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

    private int stepsInSemicircle;
    private float stepAngle;
    private float interopStep;
    #endregion
    #region Base

    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetVertexCount(maxVertCount);
        interopStep = StepLength / InteropFactor;
        StartPoint = Transform.position;
        IntialCorrectValues();
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
        if (Points.Count > 1)
        {
            for (int i = currVertCount; i < maxVertCount; i++)
                lineRenderer.SetPosition(i, Points.Last());
            Produce();
        }

        lineRenderer.enabled = false;
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
        List<Vector3> iPoinst = new List<Vector3>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            iCount = Mathf.CeilToInt(Vector3.Distance(points[i], points[i + 1]) / interopStep);
            for (int j = 0; j < iCount - 1; j++)
                iPoinst.Add((points[i + 1] - points[i]).normalized * interopStep * j + points[i]);
        }

        return iPoinst;
    }

    protected void AddVertex(Vector3 p)
    {
        if (Points.Count == 0 || Vector3.Distance(Points.Last(), p) >= StepLength)
        {
            if (CheckCorrect(p))
            {
                if (Points.Count > 0)
                    p = (p - Points.Last()).normalized * StepLength + Points.Last();

                Points.Add(p);
                lineRenderer.SetPosition(currVertCount, p);
                currVertCount++;
            }
        }

        for (int i = currVertCount; i < maxVertCount; i++)
            lineRenderer.SetPosition(i, p);
    }

    protected List<Vector3> GetInteropPoints(Vector3 p1, Vector3 p2)
    {
        return GetInteropPoints((new Vector3[] { p1, p2 }).ToList());
    }

    protected void AddDeathPath()
    {
        var v = (Points[Points.Count - 1] - Points[Points.Count - 2]).normalized;
        var lastpoint = Points[Points.Count - 1];
        for (int i = 1; i <= Mathf.CeilToInt(DeathDistanse / StepLength); i++)
            Points.Add(v * StepLength * i + lastpoint);
    }

    private void IntialCorrectValues()
    {
        stepsInSemicircle = Mathf.CeilToInt(Mathf.PI / (Mathf.Asin(StepLength / (2 * MinRadius))) / 2);
        stepAngle = Mathf.PI / (2 * stepsInSemicircle);
        Debug.Log(stepsInSemicircle);
    }

    protected bool CheckCorrect(Vector3 p)
    {
        if (Points.Count < 2)
            return true;

        int checkVert = Points.Count - stepsInSemicircle;
        int n = 2;
        if (checkVert < 0)
            checkVert = 0;

        var nextStep = (p - Points.Last()).normalized * StepLength + Points.Last();
        //var angle = Mathf.Acos(Vector3.Dot(p - Points.Last(), Points[Points.Count - 1] - Points[Points.Count - 2]));
        //Debug.Log(angle*Mathf.Rad2Deg+" "+minAngle);
        //if (Mathf.Abs(angle) < minAngle)
        //    return false;

        for (int i = Points.Count - 2; i >= checkVert; i--)
        {
            if (Vector3.Distance(nextStep, Points[i]) < 2 * MinRadius * Mathf.Sin(stepAngle * n))
                return false;
            n++;
        }

        return true;
    }

    #endregion

}