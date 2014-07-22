using UnityEngine;
using System.Collections;

public class Asteriod : MonoBehaviour
{
	public Transform Transform;
	public Rigidbody Rigidbody;

	private void Awake () 
	{
		Transform = transform;
		Rigidbody = rigidbody;

		float randomScale = Random.Range(.001f, .2f);
		Transform.localScale = new Vector3(randomScale, randomScale, randomScale);
		Transform.eulerAngles = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
		Rigidbody.mass = randomScale * 20;
	}
}