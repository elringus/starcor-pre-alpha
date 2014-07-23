using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MultyLauncher : Launcher
{
    #region Definition
    public int[] OrderVolley;
    public float OffsetValue;
    public float WaveDelay;
    #endregion
    #region Base

    protected override void Produce()
    {
        AddDeathPath();
        Points.Insert(0, StartPoint);
        StartCoroutine(GenVolleys());
    }

    #endregion
    #region Methods

    private IEnumerator GenVolleys()
    {
        for (int i = 0; i < OrderVolley.Length; i++ )
        {
            MakeVolley(OrderVolley[i]);
            yield return new WaitForSeconds(WaveDelay);
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
                points.Insert(0, StartPoint);
                points = GetInteropPoints(points);
                var rocky = ((GameObject)Instantiate(Prototype, points[0], Quaternion.identity));
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
    #endregion

}