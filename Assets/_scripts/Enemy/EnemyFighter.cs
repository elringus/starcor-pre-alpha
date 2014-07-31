using UnityEngine;
using System.Collections;

public class EnemyFighter : Attacker
{

    protected override void Awake()
    {
        OwnType = global::OwnType.Allien;
        base.Awake();
    }

	protected override void Start ()
	{
		base.Start();
		Randomize();
	}

	private void Randomize ()
	{
		navAgent.speed += Random.Range(-.15f, .2f);
		navAgent.acceleration += Random.Range(-1.5f, 1f);

		//float randScale = Random.Range(.1f, .3f);
		//Transform.localScale = new Vector3(randScale, randScale, randScale);
	}
}