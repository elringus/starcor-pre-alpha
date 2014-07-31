using UnityEngine;
using System.Collections;

public class SGUI : MonoBehaviour
{
	public dfButton buttonRestart;

	private void Awake () 
	{
		buttonRestart.Click += (c, e) =>
		{
			Application.LoadLevel(Application.loadedLevel);
		};
	}

	private void Update () 
	{
    	
	}
}