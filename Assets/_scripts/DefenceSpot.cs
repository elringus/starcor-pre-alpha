using UnityEngine;
using System.Collections;

public class DefenceSpot : MonoBehaviour
{
	public Tower RocketLauncher;
	public Tower RocketVolley;
	public Tower LaserBeam;

	[HideInInspector]
	public dfSprite TowerIcon;
	[HideInInspector]
	public Tower CurrentTower;

	private void Awake () 
	{
		TowerIcon = FindObjectOfType<dfGUIManager>().AddControl<dfSprite>();
		TowerIcon.SpriteName = "tex_TowerEmpty";
		TowerIcon.Pivot = dfPivotPoint.MiddleCenter;
		TowerIcon.Size = new Vector2(60, 60);
		TowerIcon.ZOrder = 0;

		TowerIcon.Click += (c, e) =>
		{
			if (CurrentTower) SelectTower();
			else
			{
				RadMenu.I.ToggleRadMenu(this);
			}
		};
	}

	private void Update () 
	{
		Vector2 guiPos = TowerIcon.GetManager().WorldPointToGUI(transform.position);
		TowerIcon.RelativePosition = new Vector2(guiPos.x - TowerIcon.Size.x / 2, guiPos.y - TowerIcon.Size.y / 2);
	}

	public void SpawnTower (TowerType towerType)
	{
		switch (towerType)
		{
			case TowerType.RocketLauncher:
				CurrentTower = GameObject.Instantiate(RocketLauncher, transform.position, Quaternion.identity) as Tower;
				TowerIcon.SpriteName = "tex_RocketLauncher";
				break;
			case TowerType.RocketVolley:
				CurrentTower = GameObject.Instantiate(RocketVolley, transform.position, Quaternion.identity) as Tower;
				TowerIcon.SpriteName = "tex_RocketVolley";
				break;
			case TowerType.LaserBeam:
				CurrentTower = GameObject.Instantiate(LaserBeam, transform.position, Quaternion.identity) as Tower;
				TowerIcon.SpriteName = "tex_LaserBeam";
				break;
		}
	}

	public void SelectTower ()
	{
		Manager.I.SelectedTower = CurrentTower;
	}
}