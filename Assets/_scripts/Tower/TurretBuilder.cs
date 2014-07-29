using UnityEngine;
using System.Collections;

public class TurretBuilder : Tower
{
	public GameObject TurretPrototype;

	protected override void StartTargeting ()
	{
	}

	protected override void OnTargeting ()
	{
	}

	protected override void CancelTargeting ()
	{
	}

	protected override void FinishTargeting ()
	{
		var p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
		GameObject.Instantiate(TurretPrototype, p, Quaternion.identity);
		Produce();
	}
}