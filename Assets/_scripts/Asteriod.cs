using UnityEngine;
using System.Collections;

public class Asteriod : MonoBehaviour
{
	public GameObject[] Prefabs;
	public Transform Transform;
	public Rigidbody Rigidbody;

	private void Awake () 
	{
		Transform = transform;
		Rigidbody = rigidbody;

		GameObject.Instantiate(Prefabs[Random.Range(0, 3)], Transform.position, Quaternion.Euler(new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180))));

		Destroy(gameObject);

		//float randomScale = Random.Range(.001f, .2f);
		//Transform.localScale = new Vector3(randomScale, randomScale, randomScale);
		//Transform.eulerAngles = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
		//Rigidbody.mass = randomScale * 20;
	}
}