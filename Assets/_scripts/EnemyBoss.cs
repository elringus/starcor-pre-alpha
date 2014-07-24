using UnityEngine;
using System.Collections;

public class EnemyBoss : Enemy
{
	public GameObject HPBarPrefab;

	private dfProgressBar hpBar;

	protected override void Start ()
	{
		base.Start();

		hpBar = FindObjectOfType<dfGUIManager>().AddPrefab(HPBarPrefab).GetComponent<dfProgressBar>();
		hpBar.Pivot = dfPivotPoint.MiddleCenter;
		hpBar.ZOrder = 1;
	}

	protected override void Update ()
	{
		base.Update();

		Vector2 guiPos = hpBar.GetManager().WorldPointToGUI(transform.position);
		hpBar.RelativePosition = new Vector2(guiPos.x - hpBar.Size.x / 2, guiPos.y - hpBar.Size.y / 2);
		hpBar.RelativePosition = new Vector2(guiPos.x - hpBar.Size.x / 2, guiPos.y - hpBar.Size.y / 2);

		hpBar.Value = Mathf.Lerp(hpBar.Value, HP / MaxHP, Time.deltaTime * 100);
	}
}