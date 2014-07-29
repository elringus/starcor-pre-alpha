using UnityEngine;
using System.Collections;

public class RadMenu : MonoBehaviour
{
	#region SINGLETON
	private static RadMenu _instance;
	public static RadMenu I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(RadMenu)) as RadMenu;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	private DefenceSpot selectedDefenceSpot;

	public dfPanel panelRadialMenu;
	public dfButton buttonRocketLauncher;
	public dfButton buttonLaserBeam;
	public dfButton buttonRocketVolley;
	public dfButton buttonTurretBuilder;

	private void Awake () 
	{
		buttonRocketLauncher.Click += (c, e) =>
		{
			selectedDefenceSpot.SpawnTower(TowerType.RocketLauncher);
			ToggleRadMenu(null);
		};

		buttonRocketVolley.Click += (c, e) =>
		{
			selectedDefenceSpot.SpawnTower(TowerType.RocketVolley);
			ToggleRadMenu(null);
		};

		buttonLaserBeam.Click += (c, e) =>
		{
			selectedDefenceSpot.SpawnTower(TowerType.LaserBeam);
			ToggleRadMenu(null);
		};

		buttonTurretBuilder.Click += (c, e) =>
		{
			selectedDefenceSpot.SpawnTower(TowerType.TurretBuilder);
			ToggleRadMenu(null);
		};
	}

	private void Update () 
	{
    	
	}

	public void ToggleRadMenu (DefenceSpot defenceSpot)
	{
		selectedDefenceSpot = defenceSpot;

		if (panelRadialMenu.IsVisible)
		{
			panelRadialMenu.IsVisible = false;
		}
		else
		{
			panelRadialMenu.RelativePosition = (Vector2)defenceSpot.TowerIcon.RelativePosition - (Vector2)panelRadialMenu.Size * .35f;
			panelRadialMenu.IsVisible = true;
			panelRadialMenu.GetComponent<dfTweenFloat>().Play();
			foreach (dfControl but in panelRadialMenu.Controls) if (but is dfButton) but.GetComponent<dfTweenVector3>().Play();
		}
	}
}