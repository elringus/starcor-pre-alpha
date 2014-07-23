using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Volley : MonoBehaviour
{
    public GameObject RocketPrototype;

    public int[] OrderVolley = { 1, 3, 5, 7, 11 };
    public float OffsetValue = 0.25f;
    public float WaveDelay = 0.2f;

    private Vector3 StartVollyePoint;
    private float InteropFactor;
    private float StepLength;
    private List<Vector3> Points { get; set; }

    public void Initialize(List<Vector3> points)
    {
        Points = points;
    }

    private List<Vector3> GetInteropPoints(List<Vector3> points )
    {
        int iCount; 
        float iStep = StepLength/InteropFactor; //length of intrepolation step
        List<Vector3> iPoinsts = new List<Vector3>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            iCount = Mathf.CeilToInt(Vector3.Distance(points[i], points[i + 1]) / iStep);
            for (int j = 0; j < iCount - 1; j++)
                iPoinsts.Add((points[i + 1] - points[i]).normalized * iStep * j + points[i]);
        }

        return iPoinsts;
    }


    private void CreateRocket()
    {
        StartVollyePoint = (Points[0] - Points[1]).normalized * 1+Points[0];
        StartCoroutine(GenVolleys());
        //var rocky = ((GameObject)Instantiate(RocketPrototype, Points[0], Quaternion.identity));
        //rocky.GetComponent<Rocket>().Initialize(GetInteropPoints(Points));
    }

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
   
}