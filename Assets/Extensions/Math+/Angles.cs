using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public static class eMath
{
    #region Angle
    public static float Angle2Dplus(Vector3 origin, Vector3 direction)
    {
        origin = new Vector3(origin.x, origin.z, 0);
        direction = new Vector3(direction.x, direction.z, 0);

        float r = Vector3.Distance(origin, direction);
        int q = Findquarter(origin, direction);

        float angle = Mathf.PI / 2 + Angle2D(origin, direction, r, q);
        if (angle >= Mathf.PI * 2)
            angle -= Mathf.PI * 2;

        return angle;
    }

    public static float Angle2D(Vector3 origin, Vector3 direction)
    {
        float a = Angle2Dplus(origin, direction);

        if (a > Mathf.PI)
            a = a - 2 * Mathf.PI;

        return a;
    }

    private static float Angle2D(Vector3 origin, Vector3 direction, float radius, int quarter)
    {
        switch (quarter)
        {
            case 1:
                return (float)Mathf.Abs(((direction.y - origin.y) / radius).ASin());
            case 2:
                return (float)(Mathf.Abs(((direction.x - origin.x) / radius).ASin()) + Mathf.PI / 2);
            case 3:
                return (float)(Mathf.Abs(((direction.y - origin.y) / radius).ASin() + Mathf.PI));
            case 4:
                return (float)(Mathf.Abs(((direction.x - origin.x) / radius)).ASin() + 3 * Mathf.PI / 2);
            default:
                return 0;
        }
    }
    
    private static int Findquarter(Vector3 origin, Vector3 direction)
    {
        if (direction.x >= origin.x && direction.y < origin.y)
            return 1;
        if (direction.x < origin.x && direction.y < origin.y)
            return 2;
        if (direction.x < origin.x && direction.y >= origin.y)
            return 3;
        if (direction.x >= origin.x && direction.y >= origin.y)
            return 4;
        return 0;
    }
    #endregion
    #region Float
    static private int defAccuracy = 0;
    static public float R(this float f,int acc)
    {
        float t=10;
        return (float)Mathf.Round(f * t.P(acc)) / t.P(acc);
    }
    
    static public float R(this float f)
    {
        return (float)f.R(defAccuracy);
    }

    static public float P(this float f, float p)
    {
        return Mathf.Pow(f, p);
    }

    static public float ASin(this float d)
    {
        if (Mathf.Abs(d) <= 1)
            return Mathf.Asin(d);
        else
            if ((Mathf.Abs(d) - 1).R(1) == 0)
                if (d > 0)
                    return Mathf.Asin(1);
                else
                    return Mathf.Asin(-1);
            else
                return float.NaN;
    }

    static public float ACos(this float d)
    {
        if (Mathf.Abs(d) <= 1)
            return Mathf.Acos(d);
        else
            if ((Mathf.Abs(d) - 1).R(1) == 0)
                if (d > 0)
                    return Mathf.Acos(1);
                else
                    return Mathf.Acos(-1);
            else
                return float.NaN;
    }
    #endregion
    #region Equation

    public static Vector2 SqrRoots(float a, float b, float c)
    {
        float d = b.P(2) - 4 * a * c;

        if (d < 0)
            return new Vector2(float.NaN, float.NaN);

        d = Mathf.Sqrt(d);

        return new Vector2((-d - b) / (2 * a),(d - b) / (2 * a));
    }

    #endregion
    #region Vector

    public static Vector3 RandomVector(float kxy, float z)
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = UnityEngine.Random.Range(-1f, 1f);

        return new Vector3(kxy * x, kxy * y, z);
    }

    public static float Magnitude2D(this Vector3 v)
    {
        return (v.x.P(2) + v.y.P(2)).P(0.5f);
    }

    #endregion
}
