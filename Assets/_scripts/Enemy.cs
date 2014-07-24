using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	[HideInInspector]
	public Transform Transform;
	[HideInInspector]
	public GameObject GameObject;

	public float HP;
	public float Damage;
	public float HitDistance;

	private NavMeshAgent navMesh;
	private Planet targetPlanet;

	private void Awake () 
	{
		Transform = transform;
		GameObject = gameObject;

		navMesh = GetComponent<NavMeshAgent>();
		targetPlanet = GameObject.Find("planet").GetComponent<Planet>();
	}

	private void Start ()
	{
		Randomize();
	}

	private void Update () 
	{
		navMesh.SetDestination(targetPlanet.Transform.position);
		if (Vector3.Distance(Transform.position, targetPlanet.Transform.position) <= HitDistance)
		{
			targetPlanet.HP -= Time.deltaTime * Damage;
		}
	}

	private void Randomize ()
	{
		navMesh.speed += Random.Range(-.15f, .2f);
		navMesh.acceleration += Random.Range(-1.5f, 1f);

		float randScale = Random.Range(.1f, .3f);
		Transform.localScale = new Vector3(randScale, randScale, randScale);
	}

    public void TakeDamage(float dmg)
    {
        HP -= dmg;
        if (HP <= 0)
            Destroy(GameObject);
    }
}