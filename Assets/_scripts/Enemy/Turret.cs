using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : Attacker
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {


    }

	protected override void Shoot ()
	{
		base.Shoot();
		Transform.LookAt((((MonoBehaviour)currTarget).transform.position));
	}
}