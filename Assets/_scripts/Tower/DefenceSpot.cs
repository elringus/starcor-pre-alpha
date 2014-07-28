using UnityEngine;
using System.Collections;

public class DefenceSpot : MonoBehaviour
{
	public Tower RocketLauncher;
	public Tower RocketVolley;
	public Tower LaserBeam;

	[HideInInspector]
	public dfRadialSprite TowerIcon;
	[HideInInspector]
	public dfSprite TowerCDIcon;
	//[HideInInspector]
	public Tower CurrentTower;

	private void Awake () 
	{
		TowerIcon = FindObjectOfType<dfGUIManager>().AddControl<dfRadialSprite>();
		TowerIcon.SpriteName = "tex_TowerEmpty";
		TowerIcon.Pivot = dfPivotPoint.MiddleCenter;
		TowerIcon.Size = new Vector2(60, 60);
		TowerIcon.ZOrder = 2;

		TowerCDIcon = FindObjectOfType<dfGUIManager>().AddControl<dfSprite>();
		TowerCDIcon.SpriteName = "tex_TowerCD";
		TowerCDIcon.Pivot = dfPivotPoint.MiddleCenter;
		TowerCDIcon.Size = new Vector2(60, 60);
		TowerCDIcon.ZOrder = 1;

		TowerIcon.MouseDown += (c, e) =>
		{
			if (CurrentTower)
			{
				if (CurrentTower.Ready)
				{
					Manager.I.StartedTargetting = true;
					SelectTower();
				}
			}
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
		TowerCDIcon.RelativePosition = new Vector2(guiPos.x - TowerCDIcon.Size.x / 2, guiPos.y - TowerCDIcon.Size.y / 2);

		if (CurrentTower)
		{
			TowerIcon.FillAmount = Mathf.Lerp(TowerIcon.FillAmount, CurrentTower.Progress, Time.deltaTime * 100);
			TowerCDIcon.Color = CurrentTower.Ready ? Color.white : Color.red;
			print(CurrentTower.Ready);
		}
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