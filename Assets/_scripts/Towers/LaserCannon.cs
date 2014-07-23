using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LaserCannon : Tower
{
    #region Definition
    public float TargetingRadius;
    public float AttackRange = 10f;

    private Vector3 direction;
    private Vector3 origin;
    private LineRenderer lineRenderer;
    #endregion
    #region Base
    protected override void Awake()
    {
        base.Awake();
        origin = Transform.position;
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, origin);
        Transform.localScale = new Vector3(0, 0, 0);
    }
    protected override void StartTargeting()
    {
        InProcess = true;
        lineRenderer.enabled = true;
        transform.localScale = new Vector3(TargetingRadius * 2, 0, TargetingRadius * 2);
    }
    protected override void OnTargeting()
    {
        var p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        direction = new Vector3(p.x, 0, p.z);

        if (Vector3.Distance(origin, direction) > TargetingRadius)
            FinishTargeting();
        else
            lineRenderer.SetPosition(1, direction);
    }
    protected override void FinishTargeting()
    {
        if (!InProcess)
            return;

        lineRenderer.enabled = false;
        Transform.localScale = new Vector3(0, 0, 0);

        InProcess = false;
        Produce();
    }

    protected override void CancelTargeting()
    {
        lineRenderer.enabled = false;
        Transform.localScale = new Vector3(0, 0, 0);
        InProcess = false;
    }
    private void Produce()
    {
        base.Produce();
        var destination = (direction - origin).normalized * AttackRange + origin;
        ((GameObject)Instantiate(Prototype, origin, Quaternion.identity)).GetComponent<LaserBeam>().Instantiate(origin, destination);
    }

   
    #endregion
}