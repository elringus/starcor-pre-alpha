using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChainBeam : LaserBeam
{
    public float JumpRadius;
    public float MissPerJump;
    public int TargetsCount;
    
    //private int currVertCount;
    private int maxVertCount;
    private float jumpDelay = 0.15f;

    private List<Transform> previousTargets = new List<Transform>();
    protected override void Awake()
    {
        base.Awake();
        maxVertCount = TargetsCount + 1;
        lineRenderer.SetVertexCount(maxVertCount);
        //currVertCount = 1;
    }

    public override void Instantiate(Vector3 origin, Vector3 destination)
    {
        for (int i = 0; i < maxVertCount; i++)
            lineRenderer.SetPosition(i, origin);

        var hitTarget = GetTarget(origin, destination);
        if (hitTarget == null)
            lineRenderer.SetPosition(1, destination);
        else
        {
            previousTargets.Add(hitTarget.Value.transform);
            for (int i = 1; i < maxVertCount; i++)
                lineRenderer.SetPosition(i, previousTargets[0].position);

            FindTargets(hitTarget.Value.transform.position);
            LifeTime = jumpDelay * (previousTargets.Count+1);
            StartCoroutine(GoJump());
        }
    }
   

    private void FindTargets(Vector3 origin)
    {
        foreach (var hit in Physics.OverlapSphere(origin, JumpRadius))
        {
            if (!previousTargets.Contains(hit.transform))
                if (attack.CanAttack(hit.transform)!=null)
                {
                    previousTargets.Add(hit.transform);
                    if (previousTargets.Count <= TargetsCount)
                        FindTargets(hit.transform.position);
                    break;
                }
        }
    }

    private IEnumerator GoJump()
    {
        for (int i = 1; i < previousTargets.Count; i++)
        {
            for (int j = i+1; j < maxVertCount;j++ )
                lineRenderer.SetPosition(j, previousTargets[i].position);
            
            attack.Damage *= (1 - MissPerJump);
            attack.MakeAttack(previousTargets[i]);

                yield return new WaitForSeconds(jumpDelay);
        }
    }


}