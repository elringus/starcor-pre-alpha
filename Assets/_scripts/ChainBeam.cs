using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChainBeam : LaserBeam
{
    public float JumpRadius;
    public float MissPerJump;
    public int TargetsCount;
    
    private int maxVertCount;
    private float jumpDelay = 0.125f;
    private Vector3 lastOrigin;

    private List<Transform> previousTargets = new List<Transform>();
    protected override void Awake()
    {
        base.Awake();
        maxVertCount = TargetsCount + 1;
        lineRenderer.SetVertexCount(maxVertCount);
    }

    public override void Instantiate(Vector3 origin, Vector3 destination)
    {
        for (int i = 0; i < maxVertCount; i++)
            lineRenderer.SetPosition(i, origin);

        var hitTarget = GetTarget(origin, destination);
        if (hitTarget == null)
            lineRenderer.SetPosition(1, destination);
        else
            StartCoroutine(GoJumping());
    }

    protected override RaycastHit? GetTarget(Vector3 origin, Vector3 destination)
    {
        var hit = base.GetTarget(origin, destination);

        if(hit!=null)
        {
            previousTargets.Add(hit.Value.transform);
            lastOrigin = hit.Value.transform.position;
            for (int i = 1; i < maxVertCount; i++)
                lineRenderer.SetPosition(i, previousTargets[0].position);
        }

        return hit;
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

    private IEnumerator GoJumping()
    {
        while (JumpToNextTarget(lastOrigin) && previousTargets.Count <= TargetsCount)
        {
            LifeTime += jumpDelay;
            yield return new WaitForSeconds(jumpDelay);
        }
    }

    private bool JumpToNextTarget(Vector3 origin)
    {
        foreach (var hit in Physics.OverlapSphere(origin, JumpRadius))
        {
            if (!previousTargets.Contains(hit.transform))
                if (attack.CanAttack(hit.transform) != null)
                {
                    previousTargets.Add(hit.transform);
                    lastOrigin = hit.transform.position;

                    for (int i = previousTargets.Count; i < maxVertCount; i++)
                        lineRenderer.SetPosition(i, previousTargets.Last().position);

                    attack.Damage *= (1 - MissPerJump);
                    attack.MakeAttack(previousTargets.Last());

                    return true;
                }
        }

        return false;
    }

}