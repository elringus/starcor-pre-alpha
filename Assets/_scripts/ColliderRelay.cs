using UnityEngine;
using System.Collections;

public class ColliderRelay : MonoBehaviour
{
	private IRelayReciver reciver;

	private void Awake () 
	{
		//reciver = GetComponentInParent(typeof(IRelayReciver)) as IRelayReciver;
	}

	private void OnMouseDown ()
	{
		reciver = GetComponentInParent(typeof(IRelayReciver)) as IRelayReciver;
		if (reciver != null) reciver.ReceiveMouseDown();
	}
}