using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour
{
	public Transform Transform;
	public float HP;

	private void Awake () 
	{
		Transform = transform;
	}

	private void Update () 
	{
    	
	}

	private void OnGUI ()
	{
		GUILayout.Box("Planet HP: " + HP);
	}

	private void OnMouseDown ()
	{
		if (!dfGUIManager.HitTestAll(Input.mousePosition))
			DGUI.I.ToggleRadMenu();
	}
}