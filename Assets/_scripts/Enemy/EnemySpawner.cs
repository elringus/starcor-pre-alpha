using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	[HideInInspector]
	public Transform Transform;

	public GameObject EnemyPrototype;
	public float SpawnCD;

    private void Awake() 
	{
		Transform = transform;
	}

    private void Start()
	{
		StartCoroutine(Spawn(Random.Range(1, 100)));
	}

	private void Update () 
	{
    	
	}

	private IEnumerator Spawn (float delay)
	{
		yield return new WaitForSeconds(delay);

		while (true)
		{
			GameObject.Instantiate(EnemyPrototype, Transform.position, Quaternion.identity);
			yield return new WaitForSeconds(Random.Range(SpawnCD - SpawnCD / 2f, SpawnCD + SpawnCD / 2f));
		}
	}
}