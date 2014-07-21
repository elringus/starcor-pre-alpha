using UnityEngine;
using System.Collections;

public class Asteriod : MonoBehaviour
{
	public Transform Transform;

	private void Awake () 
	{
		Transform = transform;

		float randomScale = Random.Range(.001f, .2f);
		Transform.localScale = new Vector3(randomScale, randomScale, randomScale);
		Transform.eulerAngles = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
	}
}