using UnityEngine;
using System.Collections;

public class ColliderRelay : MonoBehaviour
{
	private IRelayReciver reciver;
	private bool inputting;

	private void Awake () 
	{
		//reciver = GetComponentInParent(typeof(IRelayReciver)) as IRelayReciver;
	}

	private void Update ()
	{
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			Touch touch = Input.GetTouch(0);
			Vector3 point = Camera.main.ScreenToWorldPoint(touch.position);
			
			RaycastHit hit;
			Debug.DrawRay(point, Vector3.down * 100, Color.red, 100);
			if (Physics.Raycast(point, Vector3.down, out hit, 30))
				if (collider.bounds.Contains(hit.point))
					OnMouseDown();
				else inputting = false;
		}
	}

	private void OnMouseDown ()
	{
		inputting = true;
		reciver = GetComponentInParent(typeof(IRelayReciver)) as IRelayReciver;
		if (reciver != null) reciver.ReceiveMouseDown();
	}

	public static bool IsInputting ()
	{
		foreach (var relay in FindObjectsOfType<ColliderRelay>())
			if (relay.inputting) return true;
		return false;
	}
}