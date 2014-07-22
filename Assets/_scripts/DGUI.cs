using UnityEngine;
using System.Collections;

public class DGUI : MonoBehaviour
{
	#region SINGLETON
	private static DGUI _instance;
	public static DGUI I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(DGUI)) as DGUI;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	public dfPanel panelRadialMenu;
	public dfButton buttonRocketLauncher;
	public dfButton buttonLaserBeam;
	public dfButton buttonRocketVolley;

	public GameObject RocketLauncher;
	public GameObject RocketVolley;
	public GameObject LaserBeam;

	private void Awake () 
	{
		buttonRocketLauncher.Click += (c, e) =>
		{
			panelRadialMenu.IsVisible = false;
			RocketLauncher.SetActive(true);
			LaserBeam.SetActive(false);
			RocketVolley.SetActive(false);
		};

		buttonLaserBeam.Click += (c, e) =>
		{
			panelRadialMenu.IsVisible = false;
			RocketLauncher.SetActive(false);
			LaserBeam.SetActive(true);
			RocketVolley.SetActive(false);
		};

		buttonRocketVolley.Click += (c, e) =>
		{
			panelRadialMenu.IsVisible = false;
			RocketLauncher.SetActive(false);
			LaserBeam.SetActive(false);
			RocketVolley.SetActive(true);
		};
	}

	private void Update () 
	{
    	
	}

	public void ToggleRadMenu ()
	{
		if (panelRadialMenu.IsVisible)
		{
			panelRadialMenu.IsVisible = false;
		}
		else
		{
			panelRadialMenu.RelativePosition = panelRadialMenu.GetManager().ScreenToGui(Input.mousePosition) - (Vector2)panelRadialMenu.Size * .5f;
			panelRadialMenu.IsVisible = true;
			panelRadialMenu.GetComponent<dfTweenFloat>().Play();
			foreach (dfControl but in panelRadialMenu.Controls) if (but is dfButton) but.GetComponent<dfTweenVector3>().Play();
		}
	}
}