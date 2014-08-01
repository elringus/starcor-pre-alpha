using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class HommingRocket : Rocket
{
    public float SearchRadius;
    public float SearchPeriod;

    public float HomingAcceleration;

    private bool isTrajectoryFlight;
    private float lifeTime = 3;
    private IAttackable currTarget;

    protected override void Start()
    {
        base.Start();
        isTrajectoryFlight = true;
        StartCoroutine(Homming());
    }

    protected override void Update()
    {
        if (isTrajectoryFlight)
            base.Update();
        else
          if(currTarget==null)
          {
              Transform.Translate(new Vector3(0, 0, 1) * Speed * Time.deltaTime);
              lifeTime -= Time.deltaTime;
              if (lifeTime <= 0)
                  Explode(attack, null);
          }
    }

    private IEnumerator Homming()
    {
        while (true)
        {
            if (FindTarget())
                Move();

            yield return new WaitForSeconds(SearchPeriod);
        }
    }

    private void SwitchToHomingMode()
    {
        iTween.Stop(GameObject);
        isTrajectoryFlight = false;
        Speed += HomingAcceleration;
        trailRenderer.startWidth = 0.33f;
        trailRenderer.endWidth = 0.05f;
    }

    private void Move()
    {
        if (isTrajectoryFlight)
            SwitchToHomingMode();

        Vector3 targetPoint = ((MonoBehaviour)currTarget).transform.position;
        float dt = Vector3.Distance(targetPoint, Transform.position) / Speed;
        float angularDistance = Math.Abs(Transform.rotation.eulerAngles.y - eMath.Angle2Dplus(Transform.position, targetPoint) * Mathf.Rad2Deg);
        if (angularDistance > 180) angularDistance = 360 - angularDistance;
        GameObject.MoveTo(targetPoint, dt, 0, EaseType.linear);
        GameObject.LookTo(targetPoint, angularDistance / AngularSpeed, 0);
    }

    protected virtual bool FindTarget()
    {
        IAttackable possibleTarget = null;
        IAttackable target = null;
        bool miss = true;
        foreach (var hit in Physics.OverlapSphere(Transform.position, SearchRadius))
        {
            target = attack.CanAttack(hit.transform);
            if (target != null)
            {
                if (currTarget == null)
                {
                    currTarget = target;
                    miss = false;
                    break;
                }
                else
                {
                    if (currTarget == target)
                    {
                        miss = false;
                        break;
                    }
                    else
                        if (possibleTarget == null)
                            possibleTarget = target;
                }
            }
        }

        if (miss)
            currTarget = possibleTarget;

        return currTarget!=null;
    }
}