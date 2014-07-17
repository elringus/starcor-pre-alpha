using UnityEngine;
using System.Collections;

public class Trajectory : MonoBehaviour
{
	#region SINGLETON
	private static Trajectory _instance;
	public static Trajectory I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(Trajectory)) as Trajectory;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	private void Awake () 
	{
    	
	}

	private void Update () 
	{
    	
	}
}