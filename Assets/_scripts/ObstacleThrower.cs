using UnityEngine;
using System.Collections;

public class ObstacleThrower : MonoBehaviour
{
	public float ThrowPower;

	private void Awake () 
	{
    	
	}

	private void Update () 
	{
    	
	}

	public void OnCollisionEnter (Collision colli)
	{
		if (colli.collider.CompareTag("Obstacle"))
			colli.rigidbody.AddForceAtPosition((colli.transform.position - transform.position).normalized * ThrowPower, colli.contacts[0].point, ForceMode.Impulse);
	}
}