using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour, IAttackable
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

	public void RecieveAtatck (Attack attack)
	{
		if (attack.Fof == FOF.Foe)
		{
			HP -= attack.Damage;
		}
	}
}