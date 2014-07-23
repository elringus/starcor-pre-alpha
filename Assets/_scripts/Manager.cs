using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
	#region SINGLETON
	private static Manager _instance;
	public static Manager I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(Manager)) as Manager;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	public Tower SelectedTower;
	public bool StartedTargetting;

	private void Awake () 
	{
    	
	}

	private void Start ()
	{
		StartCoroutine(InputSampler());
	}

	private void Update () 
	{
    	
	}

	private IEnumerator InputSampler ()
	{
		while (true)
		{
			if (SelectedTower && StartedTargetting)
			{
				if (Input.GetMouseButtonDown(0)) SelectedTower.Targeting(TargetingType.Start);
				if (Input.GetMouseButton(0)) SelectedTower.Targeting(TargetingType.InProcess);
				else if (Input.GetMouseButtonUp(0)) { SelectedTower.Targeting(TargetingType.Finish); StartedTargetting = false; }
				else SelectedTower.Targeting(TargetingType.None);
			}
			yield return new WaitForSeconds(.002f);
		}
	}
}