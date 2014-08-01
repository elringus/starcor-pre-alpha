using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LaserCannon : Tower
{
    #region Definition
    public float TargetingRadius;
    public float AttackRange = 10f;
    
    public float RegenHeat;
    public float HeatPerAttack;

    private float currHeat;
    private bool inOverHeating;

    private Vector3 direction;
    private LineRenderer lineRenderer;


    #endregion
    #region Base
    protected override void Awake()
    {
        base.Awake();
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, Transform.position);
       // Transform.localScale = new Vector3(0, 0, 0);
    }
    protected override void StartTargeting()
    {
        InProcess = true;
        lineRenderer.enabled = true;
        //Transform.localScale = new Vector3(TargetingRadius * 2, 0, TargetingRadius * 2);
    }
    protected override void OnTargeting()
    {
        var p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
        direction = new Vector3(p.x, 0, p.z);

		if (Vector3.Distance(Transform.position, direction) > TargetingRadius)
            FinishTargeting();
        else
            lineRenderer.SetPosition(1, direction);
    }
    protected override void FinishTargeting()
    {
        if (!InProcess)
            return;

        lineRenderer.enabled = false;
        //Transform.localScale = new Vector3(0, 0, 0);

        InProcess = false;
        Produce();
		audio.Play();
    }

    protected override void CancelTargeting()
    {
        lineRenderer.enabled = false;
        //Transform.localScale = new Vector3(0, 0, 0);
        InProcess = false;
    }
    private void Produce()
    {
        HeatUp();
		var destination = (direction - Transform.position).normalized * AttackRange + Transform.position;
		((GameObject)Instantiate(Prototype, Transform.position, Quaternion.identity)).GetComponent<LaserBeam>().Instantiate(Transform.position, destination);
    }

    protected override void Update()
    {
        if(inOverHeating)
        {
            if (RechargeTimer > 0)
                RechargeTimer -= Time.deltaTime;
            else
            {
                RechargeTimer = 0;
                currHeat = 0;
                inOverHeating = false;
            }
        }
        else
            HeatDown();
    }

    public override bool Ready
    {
        get
        {
            return !inOverHeating;
        }
    }

    public override float Progress
    {
        get
        {
            if (inOverHeating)
                return 1 - RechargeTimer / RechargeCD;
            else
                return 1 - currHeat;
        }
    }
   
    private void HeatUp()
    {
        currHeat += HeatPerAttack;
        if (currHeat >= 1)
        {
            inOverHeating = true;
            RechargeTimer = RechargeCD;
        }
    }

    private void HeatDown()
    {
        if (currHeat > 0)
            currHeat -= RegenHeat * Time.deltaTime;
    }
   
    #endregion
}